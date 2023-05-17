<template>
	<page-container type="list"
					:customClass="pageContainerCustomClass">
		<box>
			<GenericAdminGrid :configurationKey="configurationKey"
							  :genericAdminConfiguration="genericAdminConfiguration">
			</GenericAdminGrid>
		</box>
	</page-container>
</template>

<script type="text/javascript">
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
	import GenericAdminGrid from '@/Modules/GenericAdmin/Components/GenericAdminGridComponent.vue';
	import GenericAdminConfigurationService from '@/Modules/GenericAdmin/Services/generic-admin-configuration-service';

	export default {
		components: {
			Box,
			PageContainer,
			GenericAdminGrid
		},
		mixins: [LocalizationMixin],
		i18n: {
			messages: {
				getMessagesAsync(locale) {
					return import(`@/Modules/GenericAdmin/Content/generic-admin-common.${locale}.json`);
				}
			}
		},
		computed: {
			pageContainerCustomClass() {
				return this.genericAdminConfiguration ? this.genericAdminConfiguration.getPageContainerCustomClass() : '';
			},
		},
		created() {
			this.configurationKey = this.$route.params.configurationKey;
			this.genericAdminConfiguration = GenericAdminConfigurationService.get(this.configurationKey).getConfiguration();
		},
		data() {
			return {
				genericAdminConfiguration: null,
				configurationKey: null,

				title: "",
			};
		},
		methods: {
			async onNodeResolve(node) {
				let resourcesManager = await this.genericAdminConfiguration.getOrInitResourcesManagerAsync(this.$i18n.locale.value);

				node.text = this.genericAdminConfiguration.getPageTitle(resourcesManager);
				return node;
			}
		},
		async beforeRouteEnter(to, from, next) {

			//NOTE: on beforeRouteEnter event we don't have yet the component instance
			const genericAdminConfiguration = GenericAdminConfigurationService.get(to.params.configurationKey);
			const isAccessible = typeof genericAdminConfiguration.isAccessibleAsync === 'undefined'
				|| typeof genericAdminConfiguration.isAccessibleAsync === 'function'
				&& await genericAdminConfiguration.isAccessibleAsync();

			if (!isAccessible) {
				const I18n = (await import("@/Fwamework/Culture/Services/localization-service")).I18n;
				const NotificationService = (await import("@/Fwamework/Notifications/Services/notification-service")).default;
				NotificationService.showError(I18n.t("genericAdminNotAccessible"));
				next({ name: 'default' });
			} else {
				next();
			}
		}
	}
</script>