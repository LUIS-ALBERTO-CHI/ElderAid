import HttpService from "@/Fwamework/Core/Services/http-service";

export default {
	async updateAsync(data) {
		const result = await HttpService.post(`/Protections/Update`, data
		);
		return result.data;
	},

    async stopAsync(data) {
        const result = await HttpService.post(`/Protections/Stop`, data);
        return result.data;
    },

	async createAsync(data) {
		const result = await HttpService.post(`/Protections/Create`, data);
		return result.data;
	}
}
