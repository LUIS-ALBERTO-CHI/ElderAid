<template>
    <div class="breadcrimbs">
        <div class="crumbContainerCollapsed crumbContainerHidden">
            <a v-for="(link, index) in props.crumbs" :key="getNodeKey(link)" :href="link.to"
               class="crumb" @click="nodeClicked(link)" :ref="setCrumbRef">{{link.text}}</a>
        </div>
        <div class="crumbContainerCollapsed">
            <div class="dropdown"
                 :style="{ display: hiddenCount > 0 ? 'flex' : 'none' }">
                <button><i class="fa-solid fa-ellipsis dropdown-button-icon"></i></button>
                <div class="dropdownContent">
                    <router-link v-for="(link, index) in crumbsCollapsed" :key="getNodeKey(link)" :to="link.to"
                                 class="crumb" @click="nodeClicked(link)">{{link.text}}</router-link>
                </div>
            </div>
            <span v-for="(link, index) in crumbsVisible" :key="getNodeKey(link)" class="crumb">
                <router-link v-if="crumbsVisible.length -1 != index" :to="link.to" @click="nodeClicked(link)" class="crumb-link">{{link.text}}</router-link>
                <span v-else class="crumb-no-link">{{link.text}}</span>
                <span v-if="crumbsVisible.length -1 != index" :key="index" class="crumb-separator">&nbsp;></span>
            </span>
            <!--<router-link v-for="(link, index) in crumbsVisible" :key="getNodeKey(link)" :to="link.to"
                          @click="nodeClicked(link)">{{link.text}}</router-link>-->
        </div>
    </div>
</template>


<script setup>
    import { ref, defineProps, onMounted, computed, watch } from "vue";
    import { onIntersect } from "./onIntersect";
    import BreadcrumbService from '../Services/breadcrumbs-service'

    const hiddenCount = ref(0);

    const props = defineProps({
        crumbs: { type: Array, required: true },
    });

    let crumbsRefs = [];

    const setCrumbRef = (el) => {
        if (el) {
            crumbsRefs.push(el);
        }
    };

    onMounted(() => {
        crumbsRefs.forEach((el, index) => {
            onIntersect(
                el,
                () => {
                    if (index < hiddenCount.value) {
                        hiddenCount.value = index;
                    }
                },
                () => {
                    if (index + 1 > hiddenCount.value) {
                        hiddenCount.value = index + 1;
                    }
                }
            );
        });
    });

    const crumbsCollapsed = computed(() =>
        props.crumbs.slice(0, hiddenCount.value)
    );
    const crumbsVisible = computed(() =>
        props.crumbs.slice(hiddenCount.value, props.crumbs.length)
    );

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

    .crumb-separator{
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

    .dropdownContent {
        margin-top: 35px;
        background: var(--primary-bg-color);
        display: none;
        position: absolute;
        display: none;
        flex-direction: column;
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