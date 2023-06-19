import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import OrdersMasterDataService from './Services/orders-master-data-service';


export class OrdersModule extends AbstractModule {

	async onInitAsync() {
		await OrdersMasterDataService.configureAsync();
	}

}