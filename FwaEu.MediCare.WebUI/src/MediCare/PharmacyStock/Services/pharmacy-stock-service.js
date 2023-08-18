import HttpService from "@/Fwamework/Core/Services/http-service";

export default {

    async getAllAsync(cabinetId, searchTerm, page, pageSize) {
        const response = await HttpService.post('/Stock/SearchPharmacyArticles', {
            cabinetId,
            searchTerm,
            page,
            pageSize
        });
        return response.data;
    },

    async updateAsync(data) {
        const result = await HttpService.post(`/Stock/Update`, data);
        return result.data;
    }
}
