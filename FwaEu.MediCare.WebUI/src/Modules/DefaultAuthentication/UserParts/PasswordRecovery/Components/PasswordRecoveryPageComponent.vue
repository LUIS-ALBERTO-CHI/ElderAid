<template>
	<page-container type="form">
		<box>
			<dx-form :form-data="user">
				<dx-item data-field="password" :editor-options="passwordOptions">
					<dx-required-rule :message="$t('requiredConfirmPassword')" />
				</dx-item>
				<dx-item :editor-options="passwordOptions"
						 editor-type="dxTextBox">
					<dx-label :text="$t('confirmPassword')" />
					<dx-required-rule :message="$t('requiredConfirmPassword')" />
					<dx-compare-rule :comparison-target="passwordComparison"
									 :message="$t('messageConfirmPassword')" />
				</dx-item>
			</dx-form>
			<div class="form-buttons">
				<dx-button :text="$t('updatePassword')"
						   @click="sendPasswordRecovery"
						   type="success"
						   ref="updatePasswordButton"/>
			</div>
		</box>
	</page-container>
</template>
<script>
	import { DxForm, DxItem, DxLabel, DxRequiredRule, DxCompareRule } from 'devextreme-vue/form';
	import DxButton from "devextreme-vue/button";
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import PasswordRecoveryService from "../Services/password-recovery-service";
	import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
	import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';

	export default {
		components: {
			PageContainer,
			DxForm,
			DxItem,
			DxRequiredRule,
			DxLabel,
			DxCompareRule,
			Box,
			DxButton
		},
		mixins: [LocalizationMixin],
		i18n: {
			messages: {
				getMessagesAsync(locale) {
					return import(`./Content/password-recovery-messages.${locale}.json`);
				}
			}
		},
		props: {
			userId: Number,
			guid: String
		},
		data() {
			return {
				user: {
					password: ''
				},
				passwordOptions: {
					mode: 'password'
				}
			}
		},
		created: showLoadingPanel(async function () {
		}),
		methods: {
			onPasswordEntered() {
				this.$refs.updatePasswordButton.$el.click();
			},
			sendPasswordRecovery: showLoadingPanel(async function (e) {
				let result = e.validationGroup.validate();
				if (result.isValid) {
					await PasswordRecoveryService.updatePasswordAsync(this.userId, this.guid, this.user.password);
					await AuthenticationService.logoutAsync();
					window.location.href = Configuration.application.publicUrl;
				}
			}),
			passwordComparison() {
				return this.user.password;
			},
		}
	}
</script>

<style scoped>
		.page-container .box {
			margin-top: 50px;
			max-width: 300px;
			margin: 50px auto 0;
		}
</style>
