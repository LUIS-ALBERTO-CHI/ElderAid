using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.MediCare.GenericRepositorySession;
using MySqlX.XDevAPI;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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






//IDbCommand command = new SqlCommand("SP_MDC_GetArticles");
//command.Connection = _sessionContext.NhibernateSession.Connection;
//command.CommandType = CommandType.StoredProcedure;
//var parameter = new SqlParameter();
//parameter.ParameterName = "ArticlesList";
//parameter.SqlDbType = SqlDbType.Structured;
//parameter.Value = new { ArticlesList = ids.AsTableValuedParameter };
//command.Parameters.Add(parameter);
//var result = command.ExecuteNonQuery();


// storedProcedure.SetParameter("ArticlesList", ids, NHibernateUtil.Structured("dbo.TableType"));

//await dbConnection.ExecuteAsync("[dbo].[majEventsAggPositions]"
//    , commandType: CommandType.StoredProcedure
//    , param: new
//    {
//        Positions = ids.AsTableValuedParameter("[dbo].[IDs]")
//    }
//    , commandTimeout: 60);

//await dbConnection.CloseAsync();