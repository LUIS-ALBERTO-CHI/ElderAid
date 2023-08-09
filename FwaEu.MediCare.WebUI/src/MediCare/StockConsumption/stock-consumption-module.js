import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import StockConsumptionMasterDataService from './Services/stock-consumption-master-data-service';

export class StockConsumptionModule extends AbstractModule {

	async onInitAsync() {
		await StockConsumptionMasterDataService.configureAsync();
	}

}