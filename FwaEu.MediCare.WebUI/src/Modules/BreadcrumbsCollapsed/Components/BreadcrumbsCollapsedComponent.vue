<template>
    <div class="main-container">
        <router-link v-if="parentNode !== null" :to="parentNode.to" class="breadcrumb-node-link" @click="nodeClicked(parentNode)">
            <i @click="nodeClicked(parentNode)" class="fa-solid fa-angle-left" style="color: var(--primary-text-color); font-size: 26px;"></i>
        </router-link>
        <breadcrumbs-collapsed-reduce v-if="crumbs?.length > 0" :crumbs="crumbs"></breadcrumbs-collapsed-reduce>

    </div>
</template>
<script>
    import BreadcrumbService from '../Services/breadcrumbs-service'
    import ResolveContext from '../Services/resolve-context'
	import BreadcrumbsCollapsedReduce from '@/Fwamework/Breadcrumbs/Components/BreadcrumbsCollapsedReduceComponent.vue';

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
                parentNode: null,
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
                this.$emit('change', this.breadcrumbs);
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
            }
        },
        computed: {
            breadcrumbs() {
                const crumbs = this.resolvedNodes.slice(0).reverse();
                if (crumbs.length > 0) {
                    this.crumbs = crumbs;
                    // .slice makes a copy of the array, instead of mutating the orginal
                    if (crumbs.length > 1)
                        this.parentNode = crumbs[crumbs.length - 2];
                    else
                        this.parentNode = null;
                }
                return crumbs;
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
        max-width: 80%;
    }
</style>
<style src="./Content/breadcrumbs.css" />