import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import OrdersMasterDataService from './Services/orders-master-data-service';
import PeriodicOrdersMasterDataService from './Services/periodic-orders-master-data-service';



export class OrdersModule extends AbstractModule {

	async onInitAsync() {
		await OrdersMasterDataService.configureAsync();
		await PeriodicOrdersMasterDataService.configureAsync();
	}

}