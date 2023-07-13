import HttpService from "@/Fwamework/Core/Services/http-service";

const ArticlesService = {
    async getAllBySearchAsync(searchExpression, articleFamily, page, pageSize) {
        const response = await HttpService.post('/Articles/GetAllBySearchAsync', {
            searchExpression,
            articleFamily,
            page,
            pageSize
        });
        return response.data;
    }
}

export default ArticlesService;