using FwaEu.Fwamework.Users;
using FwaEu.MediCare.GenericRepositorySession;
using FwaEu.MediCare.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using NHibernate.Transform;
using System.Net.Sockets;
using System.Net;

namespace FwaEu.MediCare.Stock.Services
{
    public class StockService : IStockService
    {
        private readonly GenericSessionContext _sessionContext;
        private readonly ICurrentUserService _currentUserService;
        public StockService(GenericSessionContext sessionContext, ICurrentUserService currentUserService)
        {
            _sessionContext = sessionContext;
            _currentUserService = currentUserService;
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
            catch (Exception ex)
            {

            }
            return new List<GetAllArticlesCabinetResponse>();
        }

        public async Task UpdateAsync(UpdateStockPost model)
        {
            var query = "exec SP_MDC_UpdateStockPharmacy :StockId, :Quantity, :UserLogin, :UserIp";

            var stockedProcedure = _sessionContext.NhibernateSession.CreateSQLQuery(query);

            var currentUserLogin = ((IApplicationPartEntityPropertiesAccessor)this._currentUserService.User.Entity).Login;
            var currentUserIp = GetCurrentIpAddress();

            stockedProcedure.SetParameter("StockId", model.StockId);
            stockedProcedure.SetParameter("Quantity", model.Quantity);
            stockedProcedure.SetParameter("UserLogin", currentUserLogin);
            stockedProcedure.SetParameter("UserIp", currentUserIp);

            await stockedProcedure.ExecuteUpdateAsync();
        }

        public static string GetCurrentIpAddress()
        {
            IPAddress[] localIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());

            IPAddress localIPAddress = localIPAddresses.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip));

            if (localIPAddress == null)
                localIPAddress = localIPAddresses.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            if (localIPAddress != null)
                return localIPAddress.ToString();
            else
                return "";
        }
    }
}