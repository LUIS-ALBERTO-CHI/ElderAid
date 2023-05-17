<template>
	<box :title="$t('title')">
		<a href="#" @click="installModulePermissions"> {{$t('installModulePermissions')}}</a>
		<process-result v-if="taskResult" :results="taskResult.results">
		</process-result>
	</box>
</template>
<script>
	import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
	import SetupService from '@/Fwamework/Setup/Services/setup-service';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import ProcessResult from '@/Fwamework/ProcessResults/Components/ProcessResultComponent.vue';
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";

	export default {
		components: {
			Box,
			ProcessResult
		},
		mixins: [LocalizationMixin],
		i18n: {
			messages: {
				getMessagesAsync(locale) {
					return import(`./Content/install-permission-messages.${locale}.json`);
				}
			}
		},
		props: {
			setupTask: Object
		},
		data() {
			return {
				taskResult: null
			};
		},
		created: showLoadingPanel(async function () {
			
		}),
		methods: {
			installModulePermissions: showLoadingPanel(async function () {
				this.taskResult = await SetupService.executeSetupTaskAsync(this.setupTask.taskName, null);
			})
		}
	}
</script>