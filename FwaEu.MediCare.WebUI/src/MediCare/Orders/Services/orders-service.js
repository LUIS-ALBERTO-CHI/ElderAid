import HttpService from "@/Fwamework/Core/Services/http-service";
import OfflineDataSynchronizationService from "@/MediCare/OfflineDataSynchronization/Services/indexed-db-service";
import OnlineService from '@/fwamework/OnlineStatus/Services/online-service';

export default {
	async getAllAsync() {
		const result = await HttpService.get('Orders');
		return result.data;
	},

	async saveAsync(data) {
		if (OnlineService.isOnline()) {
			const result = await HttpService.post(`Orders/Create`, data);
			return result.data;
		} else {
			const indixedDbService = new OfflineDataSynchronizationService('orders');
			const result = indixedDbService.addToObjectStore(data);
			return result.data;
		}
	}
}
