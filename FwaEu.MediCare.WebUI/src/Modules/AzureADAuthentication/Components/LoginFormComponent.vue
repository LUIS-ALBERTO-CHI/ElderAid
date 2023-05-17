<template>
	<div class="login-button">
		<dx-button ref="loginButton"
				   type="default"
				   :text="$t('button')"
				   @click="onLoginClickAsync" />
	</div>
</template>

<script>
	import DxButton from "devextreme-vue/button";
	import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
	import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';
	import { AuthenticationHandlerKey } from '@/Modules/AzureADAuthentication/Services/azure-ad-authentication-handler';

	export default {
		mixins: [LocalizationMixin],
		i18n: {
			messages: {
				getMessagesAsync(locale) {
					return import(`./Content/login-messages.${locale}.json`);
				}
			}
		},
		methods: {
			async onLoginClickAsync(e) {
				await AuthenticationService.loginAsync(AuthenticationHandlerKey);
			}
		},
		components: {
			DxButton
		}
	};
</script>
<style type="text/css">
	.login-page .login-button {
		text-align: center;
	}
</style>