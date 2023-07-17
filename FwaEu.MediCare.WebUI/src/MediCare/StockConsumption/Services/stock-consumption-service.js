import HttpService from "@/Fwamework/Core/Services/http-service";
import OfflineDataSynchronizationService from "@/MediCare/OfflineDataSynchronization/Services/indexed-db-service";
import OnlineService from '@/fwamework/OnlineStatus/Services/online-service';

export default {
	async getAllAsync(data) {
		const result = await HttpService.post('/Stock/StockConsumptionPatient', data
		);
		return result.data;
	},
}
