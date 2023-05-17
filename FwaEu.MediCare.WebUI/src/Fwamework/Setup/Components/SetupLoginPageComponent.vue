<template>
	<div>
		<dx-validation-group>
			<box :title="$t('title')">
				<div class="dx-field">
					<dx-text-box :placeholder="$t('login')" width="100%" v-model="loginModel.userName">
						<dx-validator>
							<dx-required-rule :message="$t('messageLogin')" />
						</dx-validator>
					</dx-text-box>
				</div>
				<div class="dx-field">
					<dx-text-box :placeholder="$t('password')"
								 width="100%"
								 mode="password"
								 v-model="loginModel.password"
								 @enter-key="onPasswordEntered">
						<dx-validator>
							<dx-required-rule :message="$t('messagePassword')" />
						</dx-validator>
					</dx-text-box>
				</div>
				<div class="dx-field login-button">
					<dx-button ref="loginButton"
							   type="default"
							   :text="$t('button')"
							   @click="onLoginClickAsync" />
				</div>
			</box>
		</dx-validation-group>
	</div>
</template>

<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import DxButton from "devextreme-vue/button";
	import DxTextBox from "devextreme-vue/text-box";
	import DxValidationGroup from "devextreme-vue/validation-group";
	import DxValidator, { DxRequiredRule } from "devextreme-vue/validator";
	import SetupAuthenticationService from '@/Fwamework/Setup/Services/setup-authentication-service';
	import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
	import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';

	export default {
		components: {
			Box,
			DxButton,
			DxTextBox,
			DxValidator,
			DxRequiredRule,
			DxValidationGroup
		},
		mixins: [LocalizationMixin],
		i18n: {
			messages: {
				getMessagesAsync(locale) {
					return import(`./Content/setup-messages.${locale}.json`);
				}
			}
		},
		data() {
			return {
				loginModel: {
					userName: null,
					password: null
				}
			};
		},
		methods: {
			onPasswordEntered() {
				this.$refs.loginButton.$el.click();
			},
			async onLoginClickAsync(e) {
				if (!e.validationGroup.validate().isValid) {
					return;
				}
				const $this = this;
				await SetupAuthenticationService.loginAsync(this.loginModel.userName, this.loginModel.password).then(() => {
					$this.$emit('logged-in');
				}).catch(error => {
					if (error.response.status === 401) {
						NotificationService.showWarning(this.$t("loginErrorMessage"));
					} else {
						throw error;
					}
				});
			}
		}
	};
</script>
