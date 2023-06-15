import HttpService from "@/Fwamework/Core/Services/http-service";

export default {
	async getAllAsync() {
		const result = await HttpService.get('Orders');
		return result.data;
	},


	async saveAsync(data) {
		console.log("Calling api save orders")
		const result = await HttpService.post(`Orders`, data);
		return result.data;
	}
}
