<template>
    <div>
        <div ref="expandableTextBox" class="expandable-text-block">
            <slot></slot>
        </div>
        <a v-if="!isExpanded" @click="expandTextBlock">Voir plus...</a>
        <a v-else @click="collapseTextBlock">Réduire</a>
    </div>
</template>

<script>

    export default {
        props: {
            visibleLines: {
                type: Number,
                default: 10
            }
        },
        data() {
            return {
                element: null,
                isExpanded: false 
            };
        },
        mounted() {
            this.element = this.$refs.expandableTextBox;
            this.initWebkitLineClamp();
        },
        methods: {
            initWebkitLineClamp() {
                this.element.style.webkitLineClamp = this.visibleLines;
            },
            expandTextBlock() {
                this.element.style.webkitLineClamp = "initial";
                this.isExpanded = true;
            },
            collapseTextBlock() {
                this.initWebkitLineClamp();
                this.isExpanded = false;
            }
        }
    }
</script>

<style type="text/css" src="./Content/expandable-text-block.css"></style>