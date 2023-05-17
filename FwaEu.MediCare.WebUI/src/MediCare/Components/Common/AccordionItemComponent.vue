<template>
    <li class="accordion__item">
        <div :class="{'accordion__trigger_active': visible, 'accordion__trigger': true }"
             @click="toggleState">

            <slot name="header"></slot>
            <i :class="{ opened: visible, 'far fa-chevron-down': true }"></i>
        </div>
        <transition name="accordion"
                    @enter="start"
                    @after-enter="end"
                    @before-leave="start"
                    @after-leave="end">

            <div class="accordion__content"
                 v-show="!visible">
                <slot name="content"></slot>
            </div>
        </transition>
    </li>
</template>

<script>
    export default {
        props: {},
        inject: ["Accordion"],
        data() {
            return {
                index: null
            };
        },
        computed: {
            visible() {
                return this.index == this.Accordion.active;
            }
        },
        methods: {
            toggleState() {
                if (this.visible) {
                    this.Accordion.active = null;
                }
                else {
                    this.Accordion.active = this.index;
                }
            },
            start(el) {
                el.style.height = el.scrollHeight + "px";
            },
            end(el) {
                el.style.height = "";
            }
        },
        created() {
            this.index = this.Accordion.count++;
        }
    };
</script>

<style lang="scss" scoped>
    .accordion__item {
        cursor: pointer;
        padding: 10px 20px 10px 10px;
        border-bottom: 1px solid var(--secondary-bg-color);
        position: relative;
    }

    .accordion__item .fa-chevron-down {
        color: var(--secondary-color);
        transition: transform 0.3s ease-in-out;
    }

    .accordion__item .fa-chevron-down.opened {
        transform: rotate(180deg);
    }

    .accordion__trigger {
        display: flex;
        justify-content: space-between;
        align-items: center;
    }

    .accordion__content {
        margin-top: 10px;
    }

    .accordion-enter-active,
    .accordion-leave-active {
        will-change: height, opacity;
        transition: height 0.3s ease-in-out, opacity 0.3s ease-in-out;
        overflow: hidden;
    }

    .accordion-enter,
    .accordion-leave-to {
        height: 0 !important;
        opacity: 0;
    }
</style>
