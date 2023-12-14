import HttpService from "@/Fwamework/Core/Services/http-service";

export default {
	async getAllAsync(data) {
		const result = await HttpService.post('/Stock/StockConsumptionPatient', data
		);
		return result.data;
	},
}
