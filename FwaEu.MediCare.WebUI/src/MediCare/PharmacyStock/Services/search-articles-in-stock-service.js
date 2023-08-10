import HttpService from "@/Fwamework/Core/Services/http-service";

const ArticlesInStockService = {
    async getAllAsync(cabinetId, searchTerm, page, pageSize) {
        const response = await HttpService.post('/Stock/SearchPharmacyArticles', {
            cabinetId,
            searchTerm,
            page,
            pageSize
        });
        return response.data;
    },
}

export default ArticlesInStockService;
