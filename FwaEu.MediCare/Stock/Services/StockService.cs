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
using FwaEu.Fwamework.Temporal;
using FwaEu.Modules.MasterData;
using FwaEu.Modules.UserNotifications;
using Microsoft.AspNetCore.SignalR;
using FwaEu.Fwamework.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.MediCare.Stock.Services
{
    public class StockService : IStockService
    {
        private readonly GenericSessionContext _sessionContext;
        private readonly ICurrentUserService _currentUserService;
		private readonly IScopedServiceProvider _scopedServiceProvider;
		public StockService(GenericSessionContext sessionContext, 
			ICurrentUserService currentUserService,
			IScopedServiceProvider scopedServiceProvider)
        {
            _sessionContext = sessionContext;
            _currentUserService = currentUserService;
			this._scopedServiceProvider = scopedServiceProvider
			?? throw new ArgumentNullException(nameof(scopedServiceProvider));
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
            var query = "exec SP_MDC_UpdateStockPharmacy :StockId, :Quantity, :UserLogin, :UserIp, :PatientId";

            var stockedProcedure = _sessionContext.NhibernateSession.CreateSQLQuery(query);

            var currentUserLogin = ((IApplicationPartEntityPropertiesAccessor)this._currentUserService.User.Entity).Login;
            var currentUserIp = GetCurrentIpAddress();

            stockedProcedure.SetParameter("StockId", model.StockId);
            stockedProcedure.SetParameter("Quantity", model.Quantity);
            stockedProcedure.SetParameter("UserLogin", currentUserLogin);
            stockedProcedure.SetParameter("UserIp", currentUserIp);
            stockedProcedure.SetParameter("PatientId", model.PatientId);

			await stockedProcedure.ExecuteUpdateAsync();


			var serviceProvider = this._scopedServiceProvider.GetScopeServiceProvider();
			var relatedMasterDataServices = serviceProvider.GetServices<IMasterDataRelatedEntity>();

			var hubService = serviceProvider.GetService<IHubContext<UserNotificationHub, IUserNotificationClient>>();
			var masterDataKeys = relatedMasterDataServices.Select(x => x.MasterDataKey).Distinct().ToArray();
			var now = serviceProvider.GetService<ICurrentDateTime>().Now;
			var clients = hubService.Clients.All;
			await clients.SendAsync("MasterDataChanged", new NotificationSignalRModel(Guid.NewGuid(), now, new string[] { "StockConsumptions" }));
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