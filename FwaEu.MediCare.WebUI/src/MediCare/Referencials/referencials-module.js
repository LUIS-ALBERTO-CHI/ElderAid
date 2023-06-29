import GenericAdminConfigurationService from '@/Modules/GenericAdmin/Services/generic-admin-configuration-service';
import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import BuildingsMasterDataService from './Services/buildings-master-data-service';
import ArticlesMasterDataService from './Services/articles-master-data-service';
import DosageFormConfiguration from './Services/dosage-form-configuration';
import DosageFormMasterDataService from './Services/dosage-form-master-data-service';

export class ReferencialsModule extends AbstractModule {

	async onInitAsync() {
		await BuildingsMasterDataService.configureAsync();
		await ArticlesMasterDataService.configureAsync();
		await DosageFormMasterDataService.configureAsync();

		GenericAdminConfigurationService.register(DosageFormConfiguration);
	}

}