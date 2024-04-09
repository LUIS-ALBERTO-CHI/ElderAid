<template>
	<div class="anonymous-page-layout cite-educative-layout">

		<div class="layout-content-container">

			<div class="content">
				<slot />
			</div>

		</div>

	</div>
	<loading-panel container="window">
	</loading-panel>
</template>

<script>
	import { Configuration } from '@/Fwamework/Core/Services/configuration-service';
    import LoadingPanel from '@UILibrary/Fwamework/LoadingPanel/Components/LoadingPanelComponent.vue';
	
	export default {
		components: {
            LoadingPanel
		},
		props: {
			title: String
		},
		data() {
			return {
				applicationName: Configuration.application.name
			}
		}
	}

</script>


<style type="text/css" scoped>
    .cite-educative-layout {
        height: 100%;
        margin-top: -50px;
    }


	/* bg-app */
	.anonymous-page-layout {
		width: 100%;
		background-color: #f6f6f6;
	}

		.anonymous-page-layout .footer {
			position: fixed;
			bottom: 0px;
			left: 0px;
			width: 100%;
		}

	.application-name {
		text-align: center;
		font-size: 20px;
		line-height: 44px;
		height: 44px;
		background-color: #FFFFFF;
		padding: 0px 0px;
		margin: 0px;
		background-color: #0078D4;
		color: #E3F3F8;
		box-shadow: 0 1px 3px rgba(0, 0, 0, 0.12), 0 1px 2px rgba(0, 0, 0, 0.24); /* TODO: Use variables https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/4184/*/
	}

	.layout-content-container {
		background-color: #FFFFFF;
	}
</style>
