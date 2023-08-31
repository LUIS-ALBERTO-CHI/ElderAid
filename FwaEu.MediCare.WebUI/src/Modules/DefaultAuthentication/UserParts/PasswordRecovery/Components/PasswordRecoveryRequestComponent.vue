<template>
	<div style="display: flex; flex-direction: column; row-gap: 5px;">
		<a href="#" @click="test()">{{$t('reinitializePasswordLink')}}</a>
		<span style="color: red" v-show="displayErrorMessage">{{$t('resetPasswordAlert')}}</span>
	</div>
</template>
<script>
	import { DxPopup } from 'devextreme-vue/popup';
	import DxButton from "devextreme-vue/button";
	import DxTextBox from "devextreme-vue/text-box";
	import DxValidationGroup from "devextreme-vue/validation-group";
	import DxValidator, { DxRequiredRule, DxEmailRule } from "devextreme-vue/validator";
	import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import PasswordRecoveryService from '../Services/password-recovery-service';
	import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';

	export default {
		components: {
			DxPopup,
			DxButton,
			DxTextBox,
			DxValidator,
			DxRequiredRule,
			DxEmailRule,
			DxValidationGroup
		},
		mixins: [LocalizationMixin],
		i18n: {
			messages: {
				getMessagesAsync(locale) {
					return import(`./Content/password-recovery-request-messages.${locale}.json`);
				}
			}
		},
		data() {
			return {
				popupVisible: false,
				email: "",
				displayErrorMessage: false
			};
		},
		methods: {
			showReinitializePasswordPopup() {
				this.popupVisible = true;
			},
			reinitializePasswordAsync: showLoadingPanel(async function (e) {
				if (!e.validationGroup.validate().isValid) {
					return;
				}
				await PasswordRecoveryService.reinitializePasswordAsync(this.email);
				NotificationService.showConfirmation(this.$t('mailSent'));
				this.popupVisible = false;
			}),
			test() {
				this.displayErrorMessage = true;
			}
		}
	}
</script>
