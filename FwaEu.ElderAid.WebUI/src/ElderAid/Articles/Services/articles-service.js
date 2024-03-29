import HttpService from "@/Fwamework/Core/Services/http-service";
import RecentArticlesMasterDataService from "./recent-articles-master-data-service";

const ArticlesService = {
    async getAllBySearchAsync(searchExpression, articleFamily, page, pageSize) {
        const response = await HttpService.post('/Articles/GetAllBySearchAsync', {
            searchExpression,
            articleFamily,
            page,
            pageSize
        });
        return response.data;
    },
    async fillArticlesAsync(filteredArticles) {

        let articleIds = filteredArticles.map(x => x.articleId);
        const articlesFromMasterData = await RecentArticlesMasterDataService.getByIdsAsync(articleIds);
        filteredArticles.forEach(item => {
            const article = articlesFromMasterData.find(article => article.id == item.articleId)
            item.article = article
        })

        articleIds = filteredArticles.filter(x => !x.article).map(x => x.articleId);
        const articles = await this.getByIdsAsync(articleIds);
        filteredArticles.filter(x => !x.article).forEach(item => {
            const article = articles.find(article => article.id == item.articleId)
            item.article = article
        });
        return filteredArticles;
    },

    async getByIdsAsync(ids) {
        const response = await HttpService.post('/Articles/GetAllByIdsAsync',
            ids
        );
        return response.data;
    },

    async getArticlesImageAsync(pharmaCode) {
        const response = await HttpService.get(`/Articles/${pharmaCode}/Images`);

        return response.data;
    }
}

export default ArticlesService;