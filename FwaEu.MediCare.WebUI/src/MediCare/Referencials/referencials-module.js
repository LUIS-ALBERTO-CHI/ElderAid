import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import BuildingsMasterDataService from './Services/buildings-master-data-service';

export class ReferencialsModule extends AbstractModule {

	async onInitAsync() {
		await BuildingsMasterDataService.configureAsync();
	}

}