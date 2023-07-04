import GenericAdminConfigurationService from '@/Modules/GenericAdmin/Services/generic-admin-configuration-service';
import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import BuildingsMasterDataService from './Services/buildings-master-data-service';
import ArticlesMasterDataService from './Services/articles-master-data-service';
import DosageFormConfiguration from './Services/dosage-form-configuration';
import DosageFormMasterDataService from './Services/dosage-form-master-data-service';
import CabinetsMasterDataService from './Services/cabinets-master-data-service'
import TreatmentsMasterDataService from './Services/treatments-master-data-service';

export class ReferencialsModule extends AbstractModule {

	async onInitAsync() {
		await BuildingsMasterDataService.configureAsync();
		await ArticlesMasterDataService.configureAsync();
		await DosageFormMasterDataService.configureAsync();
		await CabinetsMasterDataService.configureAsync();
		await TreatmentsMasterDataService.configureAsync();

		GenericAdminConfigurationService.register(DosageFormConfiguration);
	}

}