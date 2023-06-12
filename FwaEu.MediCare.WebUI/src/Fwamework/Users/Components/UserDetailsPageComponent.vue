<template>
	<page-container type="form">
		<box v-if="user" :menu-items="menuItems" :timeline="timelineModel">
			<general-information v-if="userPartsFormItems && userPartsFormItems.length > 0" ref="generalInformation"
								 :user="user"
								 :userPartsFormItems="userPartsFormItems"
								 :current-user="currentUser"
								 @is-admin-changed="onIsAdminChangedAsync" />
			<div class="block" v-if="visibleModelsWhichAreComponents && visibleModelsWhichAreComponents.length > 0">
				<dx-accordion :data-source="visibleModelsWhichAreComponents"
							  :collapsible="true"
							  :deferRendering="false"
							  :selectedIndex="-1"
							  selection-mode="none"
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
	import { DxAccordion } from 'devextreme-vue/accordion';
	import GeneralInformation from '@/Fwamework/Users/Components/GeneralInformationComponent.vue';
	import PageContainer from '@/Fwamework/PageContainer/Components/PageContainerComponent.vue';
	import UserService from '@/Fwamework/Users/Services/user-service';
	import UserFormatterService from '@/Fwamework/Users/Services/user-formatter-service';
	import UserPartsRegistry from '@/Fwamework/Users/Services/users-parts-registry';
	import { DxButton } from 'devextreme-vue/button';
	import DxCheckBox from 'devextreme-vue/check-box';
	import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
	import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';
	import UserEditPageMixins from '@/Fwamework/Users/Services/user-edit-page-mixins';
	import CurrentUserService from "@/Fwamework/Users/Services/current-user-service";
	import NotificationService from '@/Fwamework/Notifications/Services/notification-service';

	export default {
		components: {
			Box,
			DxAccordion,
			GeneralInformation,
			PageContainer,
			DxButton,
			DxCheckBox
		},
		mixins: [LocalizationMixin, UserEditPageMixins],
		i18n: {
			messages: {
				getMessagesAsync(locale) {
					return import(`./Content/user-details-messages.${locale}.json`);
				}
			}
		},
		data() {
			const $this = this;
			return {
				userLazy: new AsyncLazy(() => UserService.getUserByIdAsync($this.$route.params.id)),
				currentUser: null,
				timelineModel: null
			};
		},

		methods: {
			getPartHandlers() {
				return UserPartsRegistry.getAll();
			},
			async getUserAsync() {
				return this.isNew
					? this.createNewUser()
					: await this.userLazy.getValueAsync();
			},
			async getContextAsync() {
				let currentUser = await CurrentUserService.getAsync();
				this.currentUser = currentUser;
				return {
					currentPage: this,
					isNew: this.isNew,
					currentUser: currentUser
				};
			},
			bindModelByLocationAsync(model) {
				switch (model.location) {
					case 'timeline':
						this.timelineModel = model.data
						break;

					default:
						throw 'Unknown location: ' + model.location;
				}
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
			async saveUserAsync(userId, userToSave, partHandlers) {
				await UserService.saveAsync(userId, userToSave).then(() => {
					this.$router.push({
						name: 'Users'
					});
				}).catch(error => {
					if (error.response.data) {
						if (error.response.data.title === "") {
							NotificationService.showWarning(this.$t("emailInvalid"));
						}
						else {
							for (const handler of partHandlers) {
								if (handler.handleErrorAsync) {
									handler.handleErrorAsync(error.response.data);
								}
								if (error.response.data.status === 412 && error.response.data.title == handler.partName) {
									NotificationService.showWarning(this.$t(error.response.data.type));
								}
							}
						}
					}
				});
			},
			createNewUser() {
				return { id: 0, parts: {} };
			},
			async onNodeResolve(node) {
				if (this.$route.params.id) {
					const user = await this.userLazy.getValueAsync();
					node.text = UserFormatterService.getUserFullName(user);
				}
				return node;
			},
			async onIsAdminChangedAsync() {
				let visibleModels = this.getVisibleModels(this.user, this.modelData.models);
				await this.fetchModelsDataAsync(visibleModels);
				this.modelData.visibleModels = visibleModels;
			},
			modelMustBeSaved(user, model) {
				return this.isVisibleModel(user, model);
			},
			isVisibleModel(user, handlerModel) {
				return handlerModel.showForAdmin === undefined
					||
					(user.isAdmin && handlerModel.showForAdmin)
					||
					(!user.isAdmin && !handlerModel.showForAdmin);
			},
			getVisibleModels(user, handlerModels) {
				const $this = this;
				return handlerModels.filter(handlerModel => $this.isVisibleModel(user, handlerModel));
			}
		},
		computed: {
			isNew() {
				return !this.$route.params.id;
			}
		}
	}
</script>
