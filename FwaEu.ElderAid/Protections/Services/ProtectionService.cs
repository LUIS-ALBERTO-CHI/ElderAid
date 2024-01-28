using FwaEu.Fwamework.Users;
using FwaEu.ElderAid.GenericRepositorySession;
using FwaEu.ElderAid.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.ElderAid.Orders;
using FwaEu.ElderAid.Organizations;
using Microsoft.Extensions.DependencyInjection;
using FwaEu.ElderAid.GenericSession;
using FwaEu.Fwamework.Temporal;
using NHibernate.Exceptions;
using FwaEu.Modules.Data.Database;
using NHibernate.Linq;
using FwaEu.Modules.MasterData;
using FwaEu.Modules.UserNotifications;
using Microsoft.AspNetCore.SignalR;
using FwaEu.Fwamework.DependencyInjection;

namespace FwaEu.ElderAid.Protections.Services
{
    public class ProtectionService : IProtectionService
    {
        private readonly GenericSessionContext _genericsessionContext;
        private readonly MainSessionContext _mainSessionContext;
        private readonly ICurrentDateTime _currentDateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IManageGenericDbService _manageGenericDbService;
		private readonly IScopedServiceProvider _scopedServiceProvider;
		public ProtectionService(GenericSessionContext genericSessionContext,
								IScopedServiceProvider scopedServiceProvider,
									MainSessionContext mainSessionContext,
                                        ICurrentUserService currentUserService,
                                            ICurrentDateTime currentDateTime,
                                                IManageGenericDbService manageGenericDbService)
        {
            _genericsessionContext = genericSessionContext;
            _mainSessionContext = mainSessionContext;
            _currentUserService = currentUserService;
            _currentDateTime = currentDateTime;
            _manageGenericDbService = manageGenericDbService;
			this._scopedServiceProvider = scopedServiceProvider
			?? throw new ArgumentNullException(nameof(scopedServiceProvider));
		}

		public async Task CreateProtectionAsync(CreateProtectionModel model)
        {
            var query = "exec SP_MDC_CreateProtection :PatientId, :ArticleId, :StartDate, :StopDate, :PosologyExpression, :PosologyJSONDetails, :UserLogin, :UserIp";

            var stockedProcedure = _genericsessionContext.NhibernateSession.CreateSQLQuery(query);

            var currentUserLogin = ((IApplicationPartEntityPropertiesAccessor)this._currentUserService.User.Entity).Login;
            var currentUserIp = GetCurrentIpAddress();

            string posologyExpression = BuildStringFromDictionary(model.ArticleUnit, model.ProtectionDosages);

            var posologyJSONArray = model.ProtectionDosages.Select(kvp => new { Hour = kvp.Key.Hours, Quantity = kvp.Value }).ToList();
            string posologyJSONString = JsonConvert.SerializeObject(posologyJSONArray);

            stockedProcedure.SetParameter("PatientId", model.PatientId);
            stockedProcedure.SetParameter("ArticleId", model.ArticleId);
            stockedProcedure.SetParameter("StartDate", model.StartDate);
            stockedProcedure.SetParameter("StopDate", model.StopDate);
            stockedProcedure.SetParameter("PosologyExpression", posologyExpression);
            stockedProcedure.SetParameter("PosologyJSONDetails", posologyJSONString);
            stockedProcedure.SetParameter("UserLogin", currentUserLogin);
            stockedProcedure.SetParameter("UserIp", currentUserIp);

            await stockedProcedure.ExecuteUpdateAsync();

			var serviceProvider = this._scopedServiceProvider.GetScopeServiceProvider();
			var relatedMasterDataServices = serviceProvider.GetServices<IMasterDataRelatedEntity>();

			var hubService = serviceProvider.GetService<IHubContext<UserNotificationHub, IUserNotificationClient>>();
			var masterDataKeys = relatedMasterDataServices.Select(x => x.MasterDataKey).Distinct().ToArray();
			var now = serviceProvider.GetService<ICurrentDateTime>().Now;
			var clients = hubService.Clients.All;
			await clients.SendAsync("MasterDataChanged", new NotificationSignalRModel(Guid.NewGuid(), now, new string[] { "Protections" }));
		}

		public async Task UpdateProtectionAsync(UpdateProtectionModel model)
        {
            var query = "exec SP_MDC_UpdateProtection :PrescriptionId, :StartDate, :StopDate, :PosologyExpression, :PosologyJSONDetails, :UserLogin, :UserIp";

            var stockedProcedure = _genericsessionContext.NhibernateSession.CreateSQLQuery(query);

            var currentUserLogin = ((IApplicationPartEntityPropertiesAccessor)this._currentUserService.User.Entity).Login;
            var currentUserIp = GetCurrentIpAddress();

            string posologyExpression = BuildStringFromDictionary(model.ArticleUnit ,model.ProtectionDosages);

            var posologyJSONArray = model.ProtectionDosages.Select(kvp => new { Hour = kvp.Key.Hours, Quantity = kvp.Value }).ToList();
            string posologyJSONString= JsonConvert.SerializeObject(posologyJSONArray);

            stockedProcedure.SetParameter("PrescriptionId", model.ProtectionId);
            stockedProcedure.SetParameter("StartDate", model.StartDate);
            stockedProcedure.SetParameter("StopDate", model.StopDate);
            stockedProcedure.SetParameter("PosologyExpression", posologyExpression);
            stockedProcedure.SetParameter("PosologyJSONDetails", posologyJSONString);
            stockedProcedure.SetParameter("UserLogin", currentUserLogin);
            stockedProcedure.SetParameter("UserIp", currentUserIp);

            await stockedProcedure.ExecuteUpdateAsync();
            await CancelPeriodicOrderAsync(model.PatientId);

			var serviceProvider = this._scopedServiceProvider.GetScopeServiceProvider();
			var relatedMasterDataServices = serviceProvider.GetServices<IMasterDataRelatedEntity>();

			var hubService = serviceProvider.GetService<IHubContext<UserNotificationHub, IUserNotificationClient>>();
			var masterDataKeys = relatedMasterDataServices.Select(x => x.MasterDataKey).Distinct().ToArray();
			var now = serviceProvider.GetService<ICurrentDateTime>().Now;
			var clients = hubService.Clients.All;
			await clients.SendAsync("MasterDataChanged", new NotificationSignalRModel(Guid.NewGuid(), now, new string[] { "Protections" }));

		}

		public async Task StopProtectionAsync(StopProtectionModel model)
        {
            var query = "exec SP_MDC_StopProtection :PrescriptionId, :StopDate, :UserLogin, :UserIp";
            var stockedProcedure = _genericsessionContext.NhibernateSession.CreateSQLQuery(query);

            var currentUserLogin = ((IApplicationPartEntityPropertiesAccessor)this._currentUserService.User.Entity).Login;
            var currentUserIp = GetCurrentIpAddress();

            stockedProcedure.SetParameter("PrescriptionId", model.ProtectionId);
            stockedProcedure.SetParameter("StopDate", model.StopDate);
            stockedProcedure.SetParameter("UserLogin", currentUserLogin);
            stockedProcedure.SetParameter("UserIp", currentUserIp);
            
            await stockedProcedure.ExecuteUpdateAsync();
            await CancelPeriodicOrderAsync(model.PatientId);

			var serviceProvider = this._scopedServiceProvider.GetScopeServiceProvider();
			var relatedMasterDataServices = serviceProvider.GetServices<IMasterDataRelatedEntity>();

			var hubService = serviceProvider.GetService<IHubContext<UserNotificationHub, IUserNotificationClient>>();
			var masterDataKeys = relatedMasterDataServices.Select(x => x.MasterDataKey).Distinct().ToArray();
			var now = serviceProvider.GetService<ICurrentDateTime>().Now;
			var clients = hubService.Clients.All;
			await clients.SendAsync("MasterDataChanged", new NotificationSignalRModel(Guid.NewGuid(), now, new string[] { "Protections" }));

		}

		public async Task CancelPeriodicOrderAsync(int patientId)
        {
            try
            {
                var repositorySession = this._mainSessionContext.RepositorySession;
                var repository = repositorySession.Create<PeriodicOrderValidationEntityRepository>();

                var organizationRepository = repositorySession.Create<OrganizationEntityRepository>();
                var currentDbId = _manageGenericDbService.GetGenericDbId();
                var organization = await organizationRepository.GetAsync(currentDbId);

                var dateNow = _currentDateTime.Now;
                var periodicOrderValidations = repository.Query().Where(x => x.PatientId == patientId && x.OrderedOn == null && x.CreatedOn > organization.LastPeriodicityOrder);
                foreach (var periodicOrderValidation in periodicOrderValidations)
                {
                    await repository.DeleteAsync(periodicOrderValidation);
                    await repositorySession.Session.FlushAsync();

					var serviceProvider = this._scopedServiceProvider.GetScopeServiceProvider();
					var relatedMasterDataServices = serviceProvider.GetServices<IMasterDataRelatedEntity>();

					var hubService = serviceProvider.GetService<IHubContext<UserNotificationHub, IUserNotificationClient>>();
					var masterDataKeys = relatedMasterDataServices.Select(x => x.MasterDataKey).Distinct().ToArray();
					var now = serviceProvider.GetService<ICurrentDateTime>().Now;
					var clients = hubService.Clients.All;
					await clients.SendAsync("MasterDataChanged", new NotificationSignalRModel(Guid.NewGuid(), now, new string[] { "PeriodicOrders" }));

				}
			}
            catch (GenericADOException e)
            {
                DatabaseExceptionHelper.CheckForDbConstraints(e);
                throw;
            }
        }

        static string BuildStringFromDictionary(string articleUnit, Dictionary<TimeSpan, int> protectionDosages)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(articleUnit);
            foreach (var protectionDosage in protectionDosages)
            {
                sb.Append('/');
                sb.Append(protectionDosage.Value);
                sb.Append(':');
                sb.Append(protectionDosage.Key.ToString(@"h\h"));
            }

            return sb.ToString();
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