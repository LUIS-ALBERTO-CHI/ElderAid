import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { CanAdministrateReports } from '@/Modules/ReportAdmin/report-admin-permissions';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';

class ReportCategoryConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}

	getResources(locale) {
		return [import(`@/Modules/ReportMasterData/Content/report-category-messages.${locale}.json`)];
	}

	getPageTitle(resourcesManager) {
		return resourcesManager.getResource(['reportCategoriesTitle']);
	}

	getDescription(resourcesManager) {
		return resourcesManager.getResource(['reportCategoriesDescription']);
	}

	getGroupText(resourcesManager) {
		return resourcesManager.getResource(['reports']);
	}
}

export default {
	configurationKey: 'ReportCategoryEntity',
	icon: "fas fa-list-ul",
	getConfiguration: function () {
		return new ReportCategoryConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateReports);
	}
};