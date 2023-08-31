using FwaEu.MediCare.GenericRepositorySession;
using NHibernate.Transform;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Articles.Services
{
    public class ArticleService : IArticleService
    {
        private readonly GenericSessionContext _sessionContext;

        public ArticleService(GenericSessionContext sessionContext)
        {
            _sessionContext = sessionContext;
        }

        public async Task<List<GetArticlesBySearchResponse>> GetAllBySearchAsync(GetArticlesBySearchPost model)
        {
            var query = "exec SP_MDC_SearchArticle :SearchExpression, :ArticleFamily, :Page, :PageSize";

            var storedProcedure = _sessionContext.NhibernateSession.CreateSQLQuery(query);
            storedProcedure.SetParameter("SearchExpression", model.SearchExpression);
            storedProcedure.SetParameter("ArticleFamily", model.ArticleFamily);
            storedProcedure.SetParameter("Page", model.Page);
            storedProcedure.SetParameter("PageSize", model.PageSize);

            var models = await storedProcedure.SetResultTransformer(Transformers.AliasToBean<GetArticlesBySearchResponse>()).ListAsync<GetArticlesBySearchResponse>();
            return models.ToList();
        }

        
        public async Task<List<GetArticlesByIdsReponse>> GetAllByIdsAsync(int[] ids)
        {
            var query = "exec SP_MDC_GetArticles :ArticlesList";
            string arrayList = string.Join(",", ids);
            var storedProcedure = _sessionContext.NhibernateSession.CreateSQLQuery(query);
            storedProcedure.SetParameter("ArticlesList", arrayList);
            
            var models = await storedProcedure.SetResultTransformer(Transformers.AliasToBean<GetArticlesByIdsReponse>()).ListAsync<GetArticlesByIdsReponse>();
            return models.ToList();
        }
    }
}