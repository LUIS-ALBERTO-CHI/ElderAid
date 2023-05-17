import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateFarmMasterData } from '@/MediCare/FarmManager/farms-permissions';

class FarmActivityConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}

	getResources(locale) {
		return [import(`@/MediCare/FarmManager/Content/farm-activity-messages.${locale}.json`)];
	}

	getPageTitle(resourcesManager) {
		return resourcesManager.getResource(['farmActivitiesTitle']);
	}

	getGroupText(resourcesManager) {
		return resourcesManager.getResource(['farmManager']);
	}
}

export default {
	configurationKey: 'FarmActivities',
	icon: "fas fa-tractor",
	getConfiguration: function () {
		return new FarmActivityConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateFarmMasterData);
	}
};