import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateFarmMasterData } from '@/MediCare/FarmManager/farms-permissions';

class FarmAnimalSpeciesConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}

	getResources(locale) {
		return [import(`@/MediCare/FarmManager/Content/farm-animal-species-messages.${locale}.json`)];
	}

	getPageTitle(resourcesManager) {
		return resourcesManager.getResource(['farmAnimalSpeciesTitle']);
	}

	getGroupText(resourcesManager) {
		return resourcesManager.getResource(['farmManager']);
	}
}

export default {
	configurationKey: 'FarmAnimalSpecies',
	icon: "fas fa-pig",
	getConfiguration: function () {
		return new FarmAnimalSpeciesConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateFarmMasterData);
	}
};