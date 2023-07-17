<template>
    <div class="main-container">
        <router-link v-show="parentName !== undefined" :to="parentNode.to" class="breadcrumb-node-link" @click="nodeClicked(parentNode)">
            <i @click="nodeClicked(parentNode)" class="fa-solid fa-angle-left" style="color: white; font-size: 26px;"></i>
        </router-link>
        <breadcrumbs-collapsed-reduce v-if="crumbs?.length > 0" :crumbs="crumbs"></breadcrumbs-collapsed-reduce>
    </div>
</template>
<script>
    import BreadcrumbService from '../Services/breadcrumbs-service'
    import ResolveContext from '../Services/resolve-context'
    import BreadcrumbsCollapsedReduce from '@/Modules/BreadcrumbsCollapsed/Components/BreadcrumbsCollapsedReduceComponent.vue';

    export default {
        components: {
            BreadcrumbsCollapsedReduce
        },
        props: {
            nodeSeparator: {
                type: String,
                default: '>'
            }
        },
        data() {
            return {
                parentNode: { text: '', to: '/', parentNode: '' },
                resolvedNodes: [],
                onRouteProcessedListener: BreadcrumbService.onRouteProcessed(this.onRouteProcessed),
                crumbs: []
            }
        },
        watch: {
            async $route(to) {
                await this.resolveBreadcrumb(to);
            }
        },
        async mounted() {
            await this.resolveBreadcrumb(this.$route);
        },
        methods: {
            getNodeKey(node) {
                return JSON.stringify(node.to);
            },
            async resolveBreadcrumb(route) {
                const context = new ResolveContext(this.$router, this.$i18n);
                await BreadcrumbService.processRouteAsync(route, context);
                this.$emit('change', this.breadcrumbs);
            },
            onRouteProcessed(resolved) {
                this.resolvedNodes = resolved.breadcrumbNodes;
            },
            notLastElement(index) {
                return index < this.breadcrumbs.length - 1;
            },
            notFirstElement(index) {
                return index !== 0;
            },
            getLinkText(text, index) {
                if (this.notLastElement(index) && this.notFirstElement(index)) {
                    return "...";
                }
                return text;
            },
            isPathAbsolute(path) {
                return /^(?:\/\/|[a-z]+:\/\/)/.test(path);
            },
            nodeClicked(node) {
                BreadcrumbService.nodeClicked.emitAsync({ component: this, node });

                
                const parentName = this.parentName;
                this.parentNode.text = parentName;
                this.parentNode.to = '/' + parentName;
            }
        },
        computed: {
            breadcrumbs() {
                if (this.resolvedNodes.length > 0)
                    this.crumbs = this.resolvedNodes.slice(0).reverse();
                // .slice makes a copy of the array, instead of mutating the orginal
                return this.resolvedNodes.slice(0).reverse();
            },
            parentName() {
                return this.$route.meta.breadcrumb?.parentName;
            }
        },
        beforeUnmount() {
            this.onRouteProcessedListener();
        }
    }
</script>


<style scoped>
    .main-container {
        display: flex;
        align-items: center;
        column-gap: 10px;
      }
</style>
<style src="./Content/breadcrumbs.css" />