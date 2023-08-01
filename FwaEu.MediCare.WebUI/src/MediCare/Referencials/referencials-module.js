import GenericAdminConfigurationService from '@/Modules/GenericAdmin/Services/generic-admin-configuration-service';
import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import BuildingsMasterDataService from './Services/buildings-master-data-service';
import DosageFormConfiguration from './Services/dosage-form-configuration';
import DosageFormMasterDataService from './Services/dosage-form-master-data-service';
import CabinetsMasterDataService from './Services/cabinets-master-data-service'
import IncontinenceLevelConfiguration from './Services/incontinence-level-configuration';


export class ReferencialsModule extends AbstractModule {

	async onInitAsync() {
		await BuildingsMasterDataService.configureAsync();
		await DosageFormMasterDataService.configureAsync();
		await CabinetsMasterDataService.configureAsync();

		GenericAdminConfigurationService.register(IncontinenceLevelConfiguration);
		GenericAdminConfigurationService.register(DosageFormConfiguration);
	}

}