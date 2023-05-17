<template>
	<div class="breadcrumbs">
		<i class="dx-icon-home"></i>
		<span v-for="(link, index) in breadcrumbs" :key="getNodeKey(link)" class="breadcrumb-node">
			<router-link v-if="notLastElement(index) && !isPathAbsolute(link.to)" :to="link.to" class="breadcrumb-node-link" @click="nodeClicked(link)">{{getLinkText(link.text, index)}}</router-link>
			<a v-else-if="notLastElement(index) && isPathAbsolute(link.to)" :href="link.to" @click="nodeClicked(link)">{{getLinkText(link.text, index)}}</a>
			<span v-else :key="link.text" class="breadcrumb-node-text breadcrumb-last-node" @click="nodeClicked(link)">{{link.text}}</span>
			<span v-if="notLastElement(index)" :key="index" class="breadcrumb-node-separator">&nbsp;{{ nodeSeparator }}&nbsp;</span>
		</span>
	</div>
</template>

<script>
	import { useScreenSizeInfo } from '@/Fwamework/Utils/Services/screen-size-info'
	import BreadcrumbService from '../Services/breadcrumbs-service'
	import ResolveContext from '../Services/resolve-context'
	export default {
		props: {
			nodeSeparator: {
				type: String,
				default: '/'
			}
		},
		setup() {
			const { isXSmall } = useScreenSizeInfo();
			return { isXSmall };
		},
		data()
		{
			return {
				resolvedNodes: [],
				onRouteProcessedListener: BreadcrumbService.onRouteProcessed(this.onRouteProcessed)
			}
		},
		watch: {
			async $route(to)
			{
				await this.resolveBreadcrumb(to);
			}
		},
		async mounted()
		{
			await this.resolveBreadcrumb(this.$route);
		},
		methods: {
			getNodeKey(node) {
				return JSON.stringify(node.to);
			},
			async resolveBreadcrumb(route)
			{
				const context = new ResolveContext(this.$router, this.$i18n);
				await BreadcrumbService.processRouteAsync(route, context);
				this.$emit('change', this.breadcrumbs);
			},
			onRouteProcessed(resolved) {
				this.resolvedNodes = resolved.breadcrumbNodes;
			},
			notLastElement(index)
			{
				return index < this.breadcrumbs.length - 1;
			},
			notFirstElement(index) {
				return index !== 0;
			},
			getLinkText(text, index) {
				if (this.isXSmall && this.notLastElement(index) && this.notFirstElement(index)) {
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
			breadcrumbs()
			{
				// .slice makes a copy of the array, instead of mutating the orginal
				return this.resolvedNodes.slice(0).reverse();
			}

		},
		beforeUnmount() {
			this.onRouteProcessedListener();
		}
	}
</script>

<style src="./Content/breadcrumbs.css" />