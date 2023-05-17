<template>
	<page-container type="form">
		<box v-if="currentUser && modelData.models" :title="$t('generalInformation')">
			<general-information v-if="userPartsFormItems && userPartsFormItems.length > 0" ref="generalInformation"
								 :user="user"
								 :userPartsFormItems="userPartsFormItems"
								 :current-user="currentUser" />
			<div class="block" v-if="visibleModelsWhichAreComponents && visibleModelsWhichAreComponents.length > 0">
				<dx-accordion :data-source="visibleModelsWhichAreComponents"
							  :collapsible="true"
							  :multiple="true"
							  :deferRendering="false"
							  :selectedIndex="-1"
							  ref="accordion">
					<template #title="tabPanelModel">
						<div>
							{{ tabPanelModel.data.title }}
						</div>
					</template>
					<template #item="tabPanelModel">
						<div>
							<component :ref="tabPanelModel.data.ref"
									   :is="tabPanelModel.data.handler.createComponent()"
									   v-model="tabPanelModel.data"
									   :fetched-data="tabPanelModel.data.fetchedData" />
						</div>
					</template>
				</dx-accordion>
			</div>
			<div class="form-buttons">
				<dx-button :text="$t('save')"
						   @click="saveAsync"
						   type="success" />
			</div>
		</box>
	</page-container>
</template>
<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { DxButton } from 'devextreme-vue/button';
	import { DxAccordion } from 'devextreme-vue/accordion';
	import PageContainer from '@/Fwamework/PageContainer/Components/PageContainerComponent.vue';
	import UserService from '@/Fwamework/Users/Services/user-service';
	import UserSettingsPartsRegistry from '@/Fwamework/UserSettings/Services/user-settings-parts-registry';
	import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
	import UserEditPageMixins from '@/Fwamework/Users/Services/user-edit-page-mixins';
	import CurrentUserService from "@/Fwamework/Users/Services/current-user-service";
	import GeneralInformation from '@/Fwamework/UserSettings/Components/GeneralInformationComponent.vue';
	import { AsyncLazy } from "@/Fwamework/Core/Services/lazy-load";

	export default {
		components: {
			Box,
			DxAccordion,
			PageContainer,
			DxButton,
			GeneralInformation
		},
		mixins: [LocalizationMixin, UserEditPageMixins],
		i18n: {
			messages: {
				getMessagesAsync(locale) {
					return import(`./Content/user-settings-messages.${locale}.json`);
				}
			}
		},
		data() {
			return {
				currentUser: null,
				userLazy: new AsyncLazy(async () => {
					const currentUser = await CurrentUserService.getAsync();
					return await UserService.getUserByIdAsync(currentUser.id);
				}),
			};
		},
		methods: {
			getPartHandlers() {
				return UserSettingsPartsRegistry.getAll();
			},
			async getUserAsync() {
				return await this.userLazy.getValueAsync();
			},
			async getContextAsync() {
				let currentUser = await CurrentUserService.getAsync();
				this.currentUser = currentUser;
				return {
					currentPage: this,
					isNew: false,
					currentUser: currentUser
				};
			},
			bindModelByLocationAsync(model) {
				throw 'Unknown location: ' + model.location;
			},
			getModelUserToSave() {
				return {
					id: this.user.id,
					parts: {}
				};
			},
			getGeneralInformationComponent() {
				return this.$refs.generalInformation;
			},
			async saveUserAsync(userId, userToSave) {
				await UserService.saveAsync(userId, userToSave).then(() => {
					this.$router.go(-1);
				});
			}
		}
	}
</script>
