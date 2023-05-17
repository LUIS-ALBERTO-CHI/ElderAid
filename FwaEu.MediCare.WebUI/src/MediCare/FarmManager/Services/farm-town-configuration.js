import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateFarmMasterData } from '@/MediCare/FarmManager/farms-permissions';

class FarmTownConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}

	getResources(locale) {
		return [import(`@/MediCare/FarmManager/Content/farm-town-messages.${locale}.json`)];
	}

	getPageTitle(resourcesManager) {
		return resourcesManager.getResource(['farmTownsTitle']);
	}

	getGroupText(resourcesManager) {
		return resourcesManager.getResource(['farmManager']);
	}
	async onColumnsCreatingAsync(component, columns) {
		await super.onColumnsCreatingAsync(component, columns);
		//TODO: Issues when creating new values because required culture client side is different from required culture server side https://dev.azure.com/fwaeu/MediCare/_workitems/edit/6961 
	}
}

export default {
	configurationKey: 'FarmTowns',
	icon: "fas fa-house-day",
	getConfiguration: function () {
		return new FarmTownConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateFarmMasterData);
	}
};