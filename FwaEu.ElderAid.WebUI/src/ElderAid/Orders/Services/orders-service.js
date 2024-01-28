import HttpService from "@/Fwamework/Core/Services/http-service";
import OfflineDataSynchronizationService from "@/ElderAid/OfflineDataSynchronization/Services/indexed-db-service";
import OnlineService from '@/Fwamework/OnlineStatus/Services/online-service';

export default {
	async getAllAsync(data) {
		const result = await HttpService.post('/Orders/GetAll', data
		);
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
	},

	async validatePeriodicOrderAsync(data) {
		const result = await HttpService.post(`Orders/ValidatePeriodicOrder`, data);
		return result.data;
	},

	async cancelOrderAsync(id) {
		const result = await HttpService.post(`Orders/Cancel/${id}`);
		return result.data;
	}
}
