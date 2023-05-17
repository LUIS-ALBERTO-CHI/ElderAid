<template>
	<div class="anonymous-page-layout cite-educative-layout" >
		
		<div class="layout-content-container">

			<div class="content">
				<slot />
			</div>
			
		</div>

	</div>
</template>

<script>
	import { Configuration } from '@/Fwamework/Core/Services/configuration-service';
	
	export default {
		components: {
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

    @media screen and (max-width: 480px) {
        .cite-educative-layout {
            margin-top: 10px;
        }
    }


		.anonymous-page-layout {
			width: 100%;
			background-color: #FFFFFF;
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
		color: #C7E0F4;
		box-shadow: 0 1px 3px rgba(0, 0, 0, 0.12), 0 1px 2px rgba(0, 0, 0, 0.24); /* TODO: Use variables https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/4184/*/
	}

	.layout-content-container {
		background-color: #FFFFFF;
	}
</style>
