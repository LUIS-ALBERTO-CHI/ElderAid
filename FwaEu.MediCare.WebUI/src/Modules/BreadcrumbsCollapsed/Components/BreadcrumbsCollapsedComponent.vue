<template>
    <router-link v-show="isTurnBackEnabled !== undefined" :to="parentNode.to" class="breadcrumb-node-link" @click="nodeClicked(parentNode)">
        <i @click="nodeClicked(parentNode)" class="fa-solid fa-angle-left" style="color: white; font-size: 26px;"></i>
    </router-link>
    <div class="breadcrumbs">
        {{ }}
        <div class="breadcrimbs">
            <div :style="{ visibility: isCollapsed ? 'hidden' : 'unset' }"
                 class="crumbContainer"
                 ref="crumbsRef">
                <span v-for="(link, index) in breadcrumbs" :key="getNodeKey(link)" class="breadcrumb-node">
                    <router-link v-if="notLastElement(index) && !isPathAbsolute(link.to)" :to="link.to" class="breadcrumb-node-link" @click="nodeClicked(link)">{{link.text}}</router-link>
                    <a v-else-if="notLastElement(index) && isPathAbsolute(link.to)" :href="link.to" @click="nodeClicked(link)">{{link.text}}</a>
                    <span v-else :key="link.text" class="breadcrumb-node-text breadcrumb-last-node" @click="nodeClicked(link)">{{link.text}}</span>
                    <span v-if="notLastElement(index)" :key="index" class="breadcrumb-node-separator">&nbsp;{{ nodeSeparator }}&nbsp;</span>
                </span>

            </div>
            <div :style="{ visibility: isCollapsed ? 'unset' : 'hidden' }"
                 class="crumbContainerCollapsed">
                <div class="dropdown">
                    <button>...</button>
                    <div class="dropdownContent">
                        <span v-for="(link, index) in crumbsCollapsed" :key="getNodeKey(link)" class="breadcrumb-node">
                            <router-link :to="link.to" class="breadcrumb-node-link" @click="nodeClicked(link)">{{link.text}}</router-link>
                        </span>
                    </div>
                </div>
                <span v-for="(link, index) in crumbsVisible" :key="getNodeKey(link)" class="breadcrumb-node">
                    <router-link v-if="crumbsVisible.length -1 != index" :to="link.to" class="breadcrumb-node-link" @click="nodeClicked(link)">{{link.text}}</router-link>
                    <a v-else-if="notLastElement(index) && isPathAbsolute(link.to)" :href="link.to" @click="nodeClicked(link)">{{link.text}}</a>
                    <span v-else :key="link.text" class="breadcrumb-node-text breadcrumb-last-node" @click="nodeClicked(link)">{{link.text}}</span>
                    <span v-if="crumbsVisible.length -1 != index" :key="index" class="breadcrumb-node-separator">&nbsp;{{ nodeSeparator }}&nbsp;</span>
                </span>
            </div>
        </div>
    </div>
</template>

<script>
    import BreadcrumbService from '../Services/breadcrumbs-service'
    import { useBreacrumbsCollapsed } from '../Services/breadcrumbs-collapsed-service'
    import ResolveContext from '../Services/resolve-context'

    export default {
        props: {
            nodeSeparator: {
                type: String,
                default: '/'
            }
        },
        setup() {
            const { crumbsRef, isCollapsed } = useBreacrumbsCollapsed();

            return {
                crumbsRef,
                isCollapsed
            };
        },
        data() {
            return {
                parentNode: { text: '', to: '/', parentNode: '' },
                crumbsCollapsed: [],
                crumbsVisible: [],
                resolvedNodes: [],
                onRouteProcessedListener: BreadcrumbService.onRouteProcessed(this.onRouteProcessed)
            }
        },
        watch: {
            async $route(to) {
                await this.resolveBreadcrumb(to);
            },
            'isCollapsed'(value) {
                const crumbs = this.breadcrumbs;
                this.crumbsCollapsed = crumbs.slice(0, crumbs.length - 2);
                this.crumbsVisible = crumbs.slice(crumbs.length - 2, crumbs.length);
            }
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

                const parentName = this.isTurnBackEnabled;
                this.parentNode.text = parentName;
                this.parentNode.to = '/' + parentName;
            }
        },
        computed: {
            breadcrumbs() {
                // .slice makes a copy of the array, instead of mutating the orginal
                return this.resolvedNodes.slice(0).reverse();
            },
            isTurnBackEnabled() {
                return this.$route.meta.breadcrumb.parentName;
            }
        },
        beforeUnmount() {
            this.onRouteProcessedListener();
        }
    }
</script>


<style scoped>
    .breadcrimbs {
        position: relative;
    }

    .crumbContainer {
        display: flex;
        flex: 1;
        width: 100%;
        gap: 1rem;
        font-size: 14px;
        white-space: nowrap;
        width: min-content;
    }

    .crumbContainerCollapsed {
        display: flex;
        flex: 1;
        width: 100%;
        gap: 1rem;
        font-size: 16px;
        white-space: nowrap;
        position: absolute;
        top: 0;
    }

    .dropdown {
        display: inline-block;
        position: relative;
        height: auto;
        scrollbar-width: none;
    }

    .dropdownContent {
        background-color: white;
        display: none;
        position: absolute;
        flex-direction: column;
        width: 220px;
        overflow: auto;
        box-shadow: 0px 10px 10px 0px rgba(0, 0, 0, 0.4);
        padding-block-start: 8px;
        height: auto;
    }

    .dropdown:hover .dropdownContent {
        display: flex;
    }

    .dropdownContent .breadcrumb-node .breadcrumb-node-link {
        padding: 9px;
        margin: 9px;
        color: black;
    }
</style>
<!--<style src="./Content/breadcrumbs.css" />-->