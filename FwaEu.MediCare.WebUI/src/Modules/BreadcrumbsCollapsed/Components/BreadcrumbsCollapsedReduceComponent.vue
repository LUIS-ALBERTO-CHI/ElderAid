<template>
    <div class="breadcrimbs">
        <div class="crumbContainerCollapsed crumbContainerHidden">
            <div v-for="(link, index) in props.crumbs" :key="getNodeKey(link)" :href="link.to"
                 class="crumb" @click="nodeClicked(link)" :ref="(el) => crumbRefFn(el, index)">{{link.text}}</div>
        </div>
        <div class="crumbContainerCollapsed">
            <div class="dropdown" :style="{ display: hiddenCount > 0 ? 'flex' : 'none' }">
                <button @click="toggleCollapsed" class="dropdown-button"><i class="fa-solid fa-ellipsis dropdown-button-icon"></i></button>
                <div class="dropdownContent" v-if="showCollapsed">
                    <router-link v-for="(link, index) in crumbsCollapsed" :key="getNodeKey(link)" :to="link.to"
                                 class="crumb" @click="nodeClicked(link)">{{link.text}}</router-link>
                </div>
            </div>
            <span v-for="(link, index) in crumbsVisible" :key="getNodeKey(link)" class="crumb">
                <router-link v-if="crumbsVisible.length -1 != index" :to="link.to" @click="nodeClicked(link)" class="crumb-link">{{link.text}}</router-link>
                <span v-else class="crumb-no-link">{{link.text}}</span>
                <span v-if="crumbsVisible.length -1 != index" :key="index" class="crumb-separator">&nbsp;></span>
            </span>
        </div>
    </div>
</template>
<script setup>
    import { ref, defineProps, computed, onMounted, onBeforeUnmount } from "vue";
    import { useIntersectionList } from "../Services/useIntersectionList";
    import BreadcrumbService from '../Services/breadcrumbs-service'

    const props = defineProps({ crumbs: { type: Array, required: true } });
    const rootRef = ref();

    const { hiddenCount, crumbRefFn } = useIntersectionList({
        list: props.crumbs,
        root: rootRef,
    });

    const crumbsCollapsed = computed(() =>
        props.crumbs.slice(0, hiddenCount.value)
    );
    const crumbsVisible = computed(() =>
        props.crumbs.slice(hiddenCount.value, props.crumbs.length)
    );

    const showCollapsed = ref(false);

    const toggleCollapsed = () => {
        showCollapsed.value = !showCollapsed.value;
    }

    // Close the dropdown when clicking outside the button or the dropdown
    const closeDropdown = (event) => {
        if (!event.target.closest(".dropdown")) {
            showCollapsed.value = false;
        }
    };

    // Watch for clicks outside the dropdown and close it
    onMounted(() => {
        document.addEventListener("click", closeDropdown);
    });

    onBeforeUnmount(() => {
        document.removeEventListener("click", closeDropdown);
    });

    const getNodeKey = function (node) {
        return JSON.stringify(node.to);
    }

    const nodeClicked = function (node) {
        BreadcrumbService.nodeClicked.emitAsync({ component: this, node });
    }
</script>



<style scoped>
    .breadcrimbs {
        position: relative;
        color: white;
    }

    .crumb {
        color: #fff;
        text-decoration: none;
        font-family: sans-serif;
        font-size: 16px;
    }

    .crumb-link {
        color: #fff;
    }

    .crumb-no-link {
        color: #e5e5e5;
    }

    .crumb-separator {
        padding-left: 5px;
    }

    .crumbContainerHidden {
        position: absolute;
        transform: rotateY(180deg);
        visibility: hidden;
        margin-left: 3rem;
    }

    .crumbContainerCollapsed {
        display: flex;
        align-items: center;
        flex: 1;
        gap: 0.5rem;
        font-size: 28px;
        white-space: nowrap;
    }


    .dropdown {
        display: none;
    }

    .dropdown-button {
        background-color: transparent;
        border: none;
        display: flex;
        align-items: center;
        justify-content: center;
        color: white;
    }

    .dropdown-button-icon {
        color: white;
        font-size: 18px;
    }

    .dropdownContent {
        margin-top: 30px;
        background-color: var(--primary-bg-color);
        border: 1px solid #dee2e6;
        display: none;
        position: absolute;
        flex-direction: column;
        row-gap: 10px;
        width: auto;
        height: auto;
        overflow: auto;
        box-shadow: 0px 10px 10px 0px rgba(0, 0, 0, 0.4);
        border-radius: 4px;
        padding: 10px;
    }

        .dropdownContent .crumb:hover {
            background: #f3f3f3;
        }

    .dropdown:hover .dropdownContent {
        display: flex;
    }
</style>