<template>
	<page-container type="summary" v-if="animalCountDataSource && farmDetails">
		<toolbar :menu-items="menuItems" :menu-options="{forceMenuMode: true}">
			<div>
				<div :class="stateCssClass"></div>
				<!--TODO https://dev.azure.com/fwaeu/MediCare/_workitems/edit/5007-->
				<div class="farm-state-label">
					<h1>{{$t("state", {stateLabel: farmState}) }}</h1>
				</div>
			</div>
		</toolbar>
		<layout>
			<column>
				<general-information :farm-details="farmDetails" />
			</column>
			<column>
				<animal-count :data-source="animalCountDataSource"></animal-count>
			</column>
		</layout>
	</page-container>
</template>
<script>
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
	import Layout from '@/Modules/Layouts/Components/LayoutComponent.vue';
	import Column from '@/Modules/Layouts/Components/LayoutColumnComponent.vue';
	import GeneralInformation from "@/MediCare/FarmManager/Components/SummaryGeneralInformationComponent.vue";
	import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';
	import AnimalCount from "@/MediCare/FarmManager/Components/SummaryAnimalsCountComponent.vue";
	import NotificationService from "@/Fwamework/Notifications/Services/notification-service";
	import DialogService from '@/Fwamework/DevExtreme/Services/dialog-service';
	import Toolbar from "@/Fwamework/Toolbar/Components/ToolbarComponent.vue";
	import { hasPermissionAsync } from "@/Fwamework/Permissions/Services/current-user-permissions-service";
	import { CanDeleteFarms } from "@/MediCare/FarmManager/farms-permissions";
	import FarmGeneralInformationService from "@/MediCare/FarmManager/Services/farm-general-information-service";
	import FarmAnimalsCountService from "@/MediCare/FarmManager/Services/farm-animals-count-service";

	export default {
		components: {
			PageContainer,
			GeneralInformation,
			Layout,
			Column,
			AnimalCount,
			Toolbar
		},
		mixins: [LocalizationMixin],
		i18n: {
			messages: {
				getMessagesAsync(locale) {
					return import(`./Content/summary-messages.${locale}.json`);
				}
			}
		},
		data() {
			let $this = this;
			return {
				farmDetails: null,
				farmLazy: new AsyncLazy(() => FarmGeneralInformationService.getAsync($this.$route.params.id)),
				menuItems: [],
				animalCountDataSource: null
			}
		},
		created: showLoadingPanel(async function () {
			await Promise.all([this.loadFarmDetailsAsync(), this.loadAnimalsCountAsync()]);
		}),
		methods: {
			async loadFarmDetailsAsync() {
				let farmInfo = await this.farmLazy.getValueAsync();
				if (!farmInfo) {
					NotificationService.showError(`Farm details corresponding to the id: ${this.$route.params.id} not found`);
					return;
				}
				this.farmDetails = farmInfo;
			},
			async loadAnimalsCountAsync() {
				let farmId = this.$route.params.id;
				this.animalCountDataSource = await FarmAnimalsCountService.getAllByFarmIdAsync(farmId);
			},
			async getCurrentFarmAsync() {
				return await this.farmLazy.getValueAsync();
			},
			async onMessagesLoadedAsync() {
				let $this = this;
				if (await hasPermissionAsync(CanDeleteFarms)) {
					this.menuItems = [{
						text: $this.$t('remove'),
						async action() {
							await $this.deleteFarmAsync();
							$this.$router.push({ name: 'Farms' });
						},
						icon: "trash"
					}];
				}
			},
			async deleteFarmAsync() {
				let isDeleteConfirmed = await DialogService.confirmAsync(this.$t("askConfirmationForDelete"), this.$t("confirm"));
				if (isDeleteConfirmed) {
					await FarmGeneralInformationService.deleteAsync(this.farmDetails.id);
					NotificationService.showConfirmation(this.$t("farmDeleted"));
				}
			}
		},
		computed: {
			farmState() {
				return this.farmDetails?.closingDate ? this.$t("closed") : this.$t("opened");
			},
			stateCssClass() {
				return this.farmDetails?.closingDate ? "farm-state-closed" : "farm-state-opened";
			}
		}
	}
</script>
<style src="./Content/summary.css">
</style>