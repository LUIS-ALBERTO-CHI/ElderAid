import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateFarmMasterData } from '@/MediCare/FarmManager/farms-permissions';

class FarmTownPostalCodeConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}

	getResources(locale) {
		return [import(`@/MediCare/FarmManager/Content/farm-postal-code-messages.${locale}.json`)];
	}

	getPageTitle(resourcesManager) {
		return resourcesManager.getResource(['farmPostalCodeTitle']);
	}

	getGroupText(resourcesManager) {
		return resourcesManager.getResource(['farmManager']);
	}

}

export default {
	configurationKey: 'FarmPostalCodeEntity',
	icon: "fas fa-address-card",
	getConfiguration: function () {
		return new FarmTownPostalCodeConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateFarmMasterData);
	}
};