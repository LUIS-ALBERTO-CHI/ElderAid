<template>
	<box :title="$t('title')">
		<div v-if="users">
			<login :data-source="users" :columns="getColumns()" :display-format="displayFormat" v-model:selected-item="userSelected"></login>
			<dx-button @click="btnLoginClick" :text="$t('btnLogin')"></dx-button>
			<dx-button @click="btnLogoutClick" :text="$t('btnLogout')"></dx-button>
			<process-result v-if="taskResult" :results="taskResult.results">
			</process-result>
		</div>
	</box>
</template>
<script>
	import Login from '@/Fwamework/Users/Components/GridSelectBoxComponent.vue';
	import DxButton from "devextreme-vue/button";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
	import ProcessResult from '@/Fwamework/ProcessResults/Components/ProcessResultComponent.vue';
	import SetupService from '@/Fwamework/Setup/Services/setup-service';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
	import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';
	import { AuthenticationHandlerKey } from '@/Modules/ImpersonateAuthentication/Setup/setup-impersonate-authentication-handler';

	export default {
		components: {
			Box,
			DxButton,
			ProcessResult,
			Login
		},
		mixins: [LocalizationMixin],
		i18n: {
			messages: {
				getMessagesAsync(locale) {
					return import(`./Content/impersonate-login-messages.${locale}.json`);
				}
			}
		},
		props: {
			setupTask: Object
		},
		data() {
			return {
				users: null,
				displayFormat: '{0}, {1} {2}',
				userSelected: null,
				taskResult: null
			};
		},
		async created() {
			this.users = (await SetupService.executeSetupTaskAsync("GetAllUsers", null)).data.users;
		},
		methods: {
			getColumns() {
				const columns = [{ caption: this.$t('identity'), dataField: 'identity' }];
				if (this.users.some(u => u.parts?.application?.firstName))
					columns.push({ caption: this.$t('firstName'), dataField: 'parts.application.firstName' });
				if (this.users.some(u => u.parts?.application?.lastName))
					columns.push({ caption: this.$t('lastName'), dataField: 'parts.application.lastName' });

				return columns;
			},
			btnLoginClick: showLoadingPanel(async function () {
				this.taskResult = await AuthenticationService.loginAsync(AuthenticationHandlerKey, {
					identity: this.userSelected.identity,
				});

				let result = this.taskResult.results.contexts.find(context => context.entries.find(entry => entry.type === "Error"));
				if (typeof result === 'undefined') {
					window.location.href = Configuration.application.publicUrl;
				}
			}),
			btnLogoutClick: showLoadingPanel(async function () {
				await AuthenticationService.logoutAsync();
				window.location.href = Configuration.application.publicUrl;
			})
		}
	}
</script>