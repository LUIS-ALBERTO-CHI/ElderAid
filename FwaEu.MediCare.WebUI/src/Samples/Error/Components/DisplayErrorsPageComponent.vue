<template>
	<page-container type="summary">
		<box :title="$t('title')">
			<box title="HTTP 4XX">
				<dx-button :text="$t('error4XX')" @click="throwError(499)" type="success" class="button-spacing"></dx-button>
				<dx-button :text="$t('error400')" @click="throwError(400)" type="success" class="button-spacing"></dx-button>
				<dx-button :text="$t('error401')" @click="throwError(401)" type="success" class="button-spacing"></dx-button>
				<dx-button :text="$t('error403')" @click="throwError(403)" type="success" class="button-spacing"></dx-button>
				<dx-button :text="$t('error404')" @click="throwError(404)" type="success" class="button-spacing"></dx-button>
			</box>
			<box title="Other errors">
				<dx-button :text="$t('error5XX')" @click="throwError(500)" type="success" class="button-spacing"></dx-button>
				<dx-button :text="$t('javascriptError')" @click="throwErrorJavascript" type="success" class="button-spacing"></dx-button>
				<dx-button :text="$t('sentryError')" @click="throwErrorSentry" type="success" class="button-spacing"></dx-button>
				<dx-button :text="$t('sentrylog')" @click="logSentryInfo" type="success" class="button-spacing"></dx-button>
			</box>
		</box>
	</page-container>
</template>
<script>
	import { DxButton } from 'devextreme-vue/button';
	import LocalizationMixing from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import { Sentry } from '@/Modules/Sentry/sentry-module';

	export default {
		components: {
			DxButton,
			Box,
			PageContainer
		},
		mixins: [LocalizationMixing],
		i18n: {
			messages: {
				getMessagesAsync(locale) {
					return import(`./Content/display-errors-messages.${locale}.json`);
				}
			}
		},
		created: showLoadingPanel(async function () {

		}),
		methods: {
			throwError(errorCode) {
				throw {
					isAxiosError: true,
					response: { status: errorCode, data: "", stack: Error().stack }
				};
			},
			throwErrorJavascript() {
				throw new Error("Error javascript is thrown");
			},
			logSentryInfo() {
				Sentry.captureMessage("Something went wrong");
			},
			throwErrorSentry() {
				Sentry.addBreadcrumb({
					category: "test",
					message: "test",
					level: Sentry.Severity.Info,
				});
				this.throwErrorJavascript()
			}
		}
	}

</script>
<style>
	.button-spacing {
		margin-left: 50px;
		margin-top:0;
	}
</style>