using FwaEu.Fwamework.Users;
using FwaEu.MediCare.GenericRepositorySession;
using FwaEu.MediCare.Users;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using NHibernate.Transform;

namespace FwaEu.MediCare.Stock.Services
{
    public class StockService : IStockService
    {
        private readonly GenericSessionContext _sessionContext;

        public StockService(GenericSessionContext sessionContext)
        {
            _sessionContext = sessionContext;
        }

        public async Task<List<GetAllStockConsumptionPatientResponse>> GetAllStockConsumptionPatient(GetAllStockConsumptionPatientPost model)
        {
            var query = "exec SP_MDC_StockConsumptionPatient :PatientId, :Page, :PageSize";

            var storedProcedure = _sessionContext.NhibernateSession.CreateSQLQuery(query);
            storedProcedure.SetParameter("PatientId", model.PatientId);
            storedProcedure.SetParameter("Page", model.Page);
            storedProcedure.SetParameter("PageSize", model.PageSize);

            var models = await storedProcedure.SetResultTransformer(Transformers.AliasToBean<GetAllStockConsumptionPatientResponse>()).ListAsync<GetAllStockConsumptionPatientResponse>();
            return models.ToList();
        }

        public async Task<List<GetAllArticlesCabinetResponse>> GetAllArticlesCabinets(GetAllArticlesCabinetPost model)
        {
            var query = "exec SP_MDC_SearchPharmacyArticles :CabinetId, :SearchExpression, :Page, :PageSize";

            var storedProcedure = _sessionContext.NhibernateSession.CreateSQLQuery(query);
            storedProcedure.SetParameter("CabinetId", model.CabinetId);
            storedProcedure.SetParameter("SearchExpression", model.SearchTerm);
            storedProcedure.SetParameter("Page", model.Page);
            storedProcedure.SetParameter("PageSize", model.PageSize);

            try
            {
                var models = await storedProcedure.SetResultTransformer(Transformers.AliasToBean<GetAllArticlesCabinetResponse>()).ListAsync<GetAllArticlesCabinetResponse>();
                return models.ToList();
            }
            catch(Exception ex)
            {
                
            }
            return new List<GetAllArticlesCabinetResponse>();
        }
    }
}