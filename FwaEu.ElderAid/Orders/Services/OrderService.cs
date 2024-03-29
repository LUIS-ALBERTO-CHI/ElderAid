using FwaEu.Fwamework.Users;
using FwaEu.ElderAid.GenericRepositorySession;
using FwaEu.ElderAid.Users;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using NHibernate.Transform;
using FwaEu.Fwamework.Data.Database.Sessions;
using NHibernate.Exceptions;
using FwaEu.Modules.Data.Database;
using FwaEu.Fwamework.Temporal;
using FwaEu.ElderAid.Organizations;
using FwaEu.ElderAid.GenericSession;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Options;
using FwaEu.ElderAid.Protections;
using Microsoft.Extensions.DependencyInjection;
using FluentNHibernate.Conventions.Helpers;
using FwaEu.ElderAid.Articles;
using Serilog.Core;
using Microsoft.Extensions.Logging;
using FwaEu.ElderAid.Patients;
using FwaEu.Modules.UserNotifications;
using Microsoft.AspNetCore.SignalR;
using FwaEu.Fwamework.DependencyInjection;
using FwaEu.Modules.MasterData;

namespace FwaEu.ElderAid.Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly GenericSessionContext _genericsessionContext;
        private readonly MainSessionContext _mainSessionContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICurrentDateTime _currentDateTime;
        private readonly IManageGenericDbService _manageGenericDbService;
        private readonly string _loginRobot;
        private readonly ILogger _logger;
		private readonly IScopedServiceProvider _scopedServiceProvider;
		public OrderService(GenericSessionContext genericSessionContext,
							IScopedServiceProvider scopedServiceProvider,
								MainSessionContext mainSessionContext,
                                    ICurrentUserService currentUserService,
                                        ICurrentDateTime currentDateTime,
                                            IOptions<PeriodicOrderOptions> orderOptions,
                                                IManageGenericDbService manageGenericDbService)
        {
            _genericsessionContext = genericSessionContext;
            _mainSessionContext = mainSessionContext;
            _currentUserService = currentUserService;
            _currentDateTime = currentDateTime;
            _loginRobot = orderOptions.Value.RobotEmail;
            _manageGenericDbService = manageGenericDbService;
			this._scopedServiceProvider = scopedServiceProvider
				?? throw new ArgumentNullException(nameof(scopedServiceProvider));
		}

        public async Task<List<GetAllOrdersResponse>> GetAllAsync(GetAllOrdersPost model)
        {
            var query = "exec SP_MDC_Orders :Page, :PageSize, :PatientId";

            var storedProcedure = _genericsessionContext.NhibernateSession.CreateSQLQuery(query);
            storedProcedure.SetParameter("PatientId", model.PatientId);
            storedProcedure.SetParameter("Page", model.Page <= 0 ? 1 : model.Page);
            storedProcedure.SetParameter("PageSize", model.PageSize);

            var models = await storedProcedure.SetResultTransformer(Transformers.AliasToBean<GetAllOrdersResponse>()).ListAsync<GetAllOrdersResponse>();
            return models.ToList();
        }

        public async Task CreateOrdersAsync(CreateOrdersPost[] orders, string databaseName = null)
        {
            var query = "exec SP_MDC_AddOrder :PatientId, :ArticleId, :Quantity, :IsPeriodicOrder, :UserLogin, :UserIp, :IsBox";
            if (databaseName != null)
                _genericsessionContext.NhibernateSession.Connection.ChangeDatabase(databaseName);
            var stockedProcedure = _genericsessionContext.NhibernateSession.CreateSQLQuery(query);

            var currentUserLogin = databaseName == null ? ((IApplicationPartEntityPropertiesAccessor)this._currentUserService.User.Entity).Login
                                                        : _loginRobot;
            var currentUserIp = GetCurrentIpAddress();

            foreach (var order in orders)
            {

                stockedProcedure.SetParameter("PatientId", order.PatientId);
                stockedProcedure.SetParameter("ArticleId", order.ArticleId);
                stockedProcedure.SetParameter("Quantity", order.Quantity);
                stockedProcedure.SetParameter("IsPeriodicOrder", databaseName != null);
                stockedProcedure.SetParameter("UserLogin", currentUserLogin);
                stockedProcedure.SetParameter("UserIp", currentUserIp);
                stockedProcedure.SetParameter("IsBox", order.IsBox);

                await stockedProcedure.ExecuteUpdateAsync();

				var serviceProvider = this._scopedServiceProvider.GetScopeServiceProvider();
				var relatedMasterDataServices = serviceProvider.GetServices<IMasterDataRelatedEntity>();

				var hubService = serviceProvider.GetService<IHubContext<UserNotificationHub, IUserNotificationClient>>();
				var masterDataKeys = relatedMasterDataServices.Select(x => x.MasterDataKey).Distinct().ToArray();
				var now = serviceProvider.GetService<ICurrentDateTime>().Now;
				var clients = hubService.Clients.All;
				await clients.SendAsync("MasterDataChanged", new NotificationSignalRModel(Guid.NewGuid(), now, new string[] { "PeriodicOrders", "Orders" }));

			}
		}

        public async Task ValidatePeriodicOrderAsync(ValidatePeriodicOrderPost validatePeriodicOrder)
        {
            try
            {
                var repositorySession = this._mainSessionContext.RepositorySession;
                var repository = repositorySession.Create<PeriodicOrderValidationEntityRepository>();

                var organizationRepository = repositorySession.Create<OrganizationEntityRepository>();
                var currentDbId = _manageGenericDbService.GetGenericDbId();
                var organization = await organizationRepository.GetAsync(currentDbId);

                var currentUser = this._currentUserService.User.Entity;
                var dateNow = _currentDateTime.Now;
                foreach (var article in validatePeriodicOrder.Articles)
                {
                    var entity = repository.Query().FirstOrDefault(x => x.ArticleId == article.ArticleId && x.PatientId == validatePeriodicOrder.PatientId && x.OrderedOn == null && x.UpdatedBy.Id == currentUser.Id && x.CreatedOn > organization.LastPeriodicityOrder);
                    if (entity != null)
                    {
                        entity.Quantity = article.Quantity;
						entity.ValidatedBy = currentUser;
						entity.ValidatedOn = dateNow;
					}
                    else
                    {
                        entity = new()
                        {
                            Organization = organization,
                            PatientId = validatePeriodicOrder.PatientId,
                            ArticleId = article.ArticleId,
                            Quantity = article.Quantity,
                            DefaultQuantity = article.DefaultQuantity,
                            ValidatedBy = currentUser,
                            ValidatedOn = dateNow
                        };
                    }
                    await repository.SaveOrUpdateAsync(entity);
                    await repositorySession.Session.FlushAsync();

					var serviceProvider = this._scopedServiceProvider.GetScopeServiceProvider();
					var relatedMasterDataServices = serviceProvider.GetServices<IMasterDataRelatedEntity>();

					var hubService = serviceProvider.GetService<IHubContext<UserNotificationHub, IUserNotificationClient>>();
					var masterDataKeys = relatedMasterDataServices.Select(x => x.MasterDataKey).Distinct().ToArray();
					var now = serviceProvider.GetService<ICurrentDateTime>().Now;
					var clients = hubService.Clients.All;
					await clients.SendAsync("MasterDataChanged", new NotificationSignalRModel(Guid.NewGuid(), now, new string[] { "PeriodicOrders", "PeriodicOrderValidations" }));
				}
			}
            catch (GenericADOException e)
            {
                DatabaseExceptionHelper.CheckForDbConstraints(e);
                throw;
            }
        }


        public async Task CreatePeriodicOrderAsync(int organizationId)
        {
            var repositorySession = this._mainSessionContext.RepositorySession;
            var organizationRepository = repositorySession.Create<AdminOrganizationEntityRepository>();
            var periodicOrderValidationRepository = repositorySession.Create<PeriodicOrderValidationEntityRepository>();

            var organization = await organizationRepository.GetNoPerimeterAsync(organizationId);
            if (organization != null)
            {
                _genericsessionContext.NhibernateSession.Connection.ChangeDatabase(organization.DatabaseName);
                var protectionRepository = _genericsessionContext.RepositorySession.Create<ProtectionEntityRepository>();
                var articlesRepository = _genericsessionContext.RepositorySession.Create<ArticleEntityRepository>();
                var patientsRepository = _genericsessionContext.RepositorySession.Create<PatientEntityRepository>();

                var periodicOrderValidations = await periodicOrderValidationRepository.Query()
                                                .Where(x => x.Organization.Id == organization.Id && x.OrderedOn == null)
                                                .ToListAsync();
                var orders = periodicOrderValidations.GroupBy(x => x.ArticleId)
                                                      .Select(x => new CreateOrdersPost()
                                                      {
                                                          ArticleId = x.First().ArticleId,
                                                          PatientId = x.First().PatientId,
                                                          Quantity = x.Sum(b => b.Quantity),
                                                      })
                                                      .ToList();

                var dateNow = _currentDateTime.Now;


                var unvalidatedOrders = await protectionRepository.QueryNoPerimeter()
                                                  .ToListAsync();
                var protections = unvalidatedOrders.GroupBy(x => x.ArticleId)
                                                                    .Select(x => new CreateOrdersPost()
                                                                    {
                                                                        ArticleId = x.First().ArticleId,
                                                                        PatientId = x.First().PatientId,
                                                                        Quantity = x.Sum(b => b.QuantityPerDay),
                                                                    })
                                                                   .ToArray();
                var listArticlesIds = unvalidatedOrders.Select(order => order.ArticleId).ToArray();
                var listPatientIds = unvalidatedOrders.Select(order => order.PatientId).ToArray();

                var articlesList = await articlesRepository.QueryNoPerimeter()
                    .Where(x => listArticlesIds.Contains(x.Id))
                    .Select(articles => new
                    {
                        articles.Id,
                        articles.GroupName,
                        articles.Title,
                        articles.CountInBox,
                    })
                    .ToListAsync();

                var patientsList = await patientsRepository.QueryNoPerimeter()
                        .Where(x => listPatientIds.Contains(x.Id))
                        .Select(patient => new
                        {
                            patient.Id,
                            patient.IncontinenceLevel
                        }
                        )
                        .ToListAsync();

                foreach (var protection in protections)
                {
                    var article = articlesList.FirstOrDefault(x => x.Id == protection.ArticleId);
                    if (article == null)
                    {
                        _logger.LogError("An error ocurred while trying to find the article " + protection.ArticleId);
                        continue;
                    }
                    var patient = patientsList.FirstOrDefault(x => x.Id == protection.PatientId);
                    if (patient == null)
                    {
                        _logger.LogError("An error ocurred while trying to find the patient " + protection.PatientId);
                        continue;
                    }
                    if (patient.IncontinenceLevel == IncontinenceLevel.None)
                    {
                        continue;
                    }

                    int quantity = (int)Math.Ceiling((organization.OrderPeriodicityDays * protection.Quantity) / article.CountInBox.Value);

                    protection.Quantity = quantity;

                    if (!orders.Any(x => x.ArticleId == protection.ArticleId && x.PatientId == protection.PatientId))
                    {
                        orders.Add(protection);
                    }
                }

                if (orders.Count > 0)
                {
                    await CreateOrdersAsync(orders.ToArray(), organization.DatabaseName);
                    var dateTimeNow = _currentDateTime.Now;
                    foreach (var periodicOrderValidation in periodicOrderValidations)
                    {
                        periodicOrderValidation.OrderedOn = dateTimeNow;
                        await periodicOrderValidationRepository.SaveOrUpdateAsync(periodicOrderValidation);
                        await repositorySession.Session.FlushAsync();
                    }
                    organization.LastPeriodicityOrder = dateTimeNow;
                    await organizationRepository.SaveOrUpdateAsync(organization);
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
		}


        public async Task CancelOrderAsync(int orderId)
        {
            try
            {
                var query = "exec SP_MDC_RemoveOrder :OrderId, :UserLogin, :UserIp";
                var stockedProcedure = _genericsessionContext.NhibernateSession.CreateSQLQuery(query);

                var currentUserLogin = ((IApplicationPartEntityPropertiesAccessor)this._currentUserService.User.Entity).Login;
                var currentUserIp = GetCurrentIpAddress();

                stockedProcedure.SetParameter("OrderId", orderId);
                stockedProcedure.SetParameter("UserLogin", currentUserLogin);
                stockedProcedure.SetParameter("UserIp", currentUserIp);

                await stockedProcedure.ExecuteUpdateAsync();

				var serviceProvider = this._scopedServiceProvider.GetScopeServiceProvider();
				var relatedMasterDataServices = serviceProvider.GetServices<IMasterDataRelatedEntity>();

				var hubService = serviceProvider.GetService<IHubContext<UserNotificationHub, IUserNotificationClient>>();
				var masterDataKeys = relatedMasterDataServices.Select(x => x.MasterDataKey).Distinct().ToArray();
				var now = serviceProvider.GetService<ICurrentDateTime>().Now;
				var clients = hubService.Clients.All;
				await clients.SendAsync("MasterDataChanged", new NotificationSignalRModel(Guid.NewGuid(), now, new string[] { "Orders" }));

			}
			catch (GenericADOException e)
            {
                DatabaseExceptionHelper.CheckForDbConstraints(e);
                throw;
            }
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