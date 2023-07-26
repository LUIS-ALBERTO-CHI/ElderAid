﻿using FwaEu.Fwamework.Users;
using FwaEu.MediCare.GenericRepositorySession;
using FwaEu.MediCare.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Temporal;
using FwaEu.MediCare.GenericSession;
using System.Net;
using System.Net.Sockets;
using FwaEu.MediCare.Orders;
using Microsoft.Extensions.Options;
using NHibernate.Criterion;

namespace FwaEu.MediCare.Protections.Services
{
    public class ProtectionService : IProtectionService
    {
        private readonly GenericSessionContext _genericsessionContext;
        private readonly MainSessionContext _mainSessionContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICurrentDateTime _currentDateTime;
        private readonly IManageGenericDbService _manageGenericDbService;
        private readonly string _loginRobot;
        public ProtectionService(GenericSessionContext genericSessionContext,
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
            _manageGenericDbService = manageGenericDbService;
        }

        //public async Task<List<GetAllOrdersResponse>> GetAllAsync(GetAllOrdersPost model)
        //{
        //    var query = "exec SP_MDC_Orders :Page, :PageSize, :PatientId";

        //    var storedProcedure = _genericsessionContext.NhibernateSession.CreateSQLQuery(query);
        //    storedProcedure.SetParameter("PatientId", model.PatientId);
        //    storedProcedure.SetParameter("Page", model.Page);
        //    storedProcedure.SetParameter("PageSize", model.PageSize);

        //    var models = await storedProcedure.SetResultTransformer(Transformers.AliasToBean<GetAllOrdersResponse>()).ListAsync<GetAllOrdersResponse>();
        //    return models.ToList();
        //}

        //public async Task CreateOrdersAsync(CreateOrdersPost[] orders, string databaseName = null)
        //{
        //    var query = "exec SP_MDC_AddOrder :PatientId, :ArticleId, :Quantity, :UserLogin, :UserIp";
        //    if (databaseName != null)
        //        _genericsessionContext.NhibernateSession.Connection.ChangeDatabase(databaseName);
        //    var stockedProcedure = _genericsessionContext.NhibernateSession.CreateSQLQuery(query);

        //    var currentUserLogin = databaseName == null ? ((IApplicationPartEntityPropertiesAccessor)this._currentUserService.User.Entity).Login
        //                                                : _loginRobot;
        //    var currentUserIp = GetCurrentIpAddress();

        //    foreach (var order in orders)
        //    {

        //        stockedProcedure.SetParameter("PatientId", order.PatientId);
        //        stockedProcedure.SetParameter("ArticleId", order.ArticleId);
        //        stockedProcedure.SetParameter("Quantity", order.Quantity);
        //        stockedProcedure.SetParameter("UserLogin", currentUserLogin);
        //        stockedProcedure.SetParameter("UserIp", currentUserIp);

        //        await stockedProcedure.ExecuteUpdateAsync();
        //    }
        //}

        //public async Task ValidatePeriodicOrderAsync(ValidatePeriodicOrderPost validatePeriodicOrder)
        //{
        //    try
        //    {
        //        var repositorySession = this._mainSessionContext.RepositorySession;
        //        var repository = repositorySession.Create<PeriodicOrderValidationEntityRepository>();

        //        var organizationRepository = repositorySession.Create<OrganizationEntityRepository>();
        //        var currentDbId = _manageGenericDbService.GetGenericDbId();
        //        var organization = await organizationRepository.GetAsync(currentDbId);

        //        var currentUser = this._currentUserService.User.Entity;
        //        var dateNow = _currentDateTime.Now;
        //        foreach (var article in validatePeriodicOrder.Articles)
        //        {
        //            var entity = repository.Query().FirstOrDefault(x => x.ArticleId == article.ArticleId && x.PatientId == validatePeriodicOrder.PatientId && x.OrderedOn == null && x.UpdatedBy.Id == currentUser.Id);
        //            if (entity != null)
        //            {
        //                entity.Quantity = article.Quantity;
        //            }
        //            else
        //            {
        //                entity = new()
        //                {
        //                    Organization = organization,
        //                    PatientId = validatePeriodicOrder.PatientId,
        //                    ArticleId = article.ArticleId,
        //                    Quantity = article.Quantity,
        //                    DefaultQuantity = article.DefaultQuantity,
        //                    ValidatedBy = currentUser,
        //                    ValidatedOn = dateNow
        //                };

        //            }
        //            await repository.SaveOrUpdateAsync(entity);
        //            await repositorySession.Session.FlushAsync();
        //        }
        //    }
        //    catch (GenericADOException e)
        //    {
        //        DatabaseExceptionHelper.CheckForDbConstraints(e);
        //        throw;
        //    }
        //}

        //public async Task CreatePeriodicOrderAsync(int organizationId)
        //{
        //    var repositorySession = this._mainSessionContext.RepositorySession;
        //    var organizationRepository = repositorySession.Create<AdminOrganizationEntityRepository>();
        //    var periodicOrderValidationRepository = repositorySession.Create<PeriodicOrderValidationEntityRepository>();

        //    var organization = await organizationRepository.GetNoPerimeterAsync(organizationId);
        //    if (organization != null)
        //    {
        //        var periodicOrderValidations = await periodicOrderValidationRepository.Query()
        //                                        .Where(x => x.Organization.Id == organization.Id && x.OrderedOn == null)
        //                                        .ToListAsync();
        //        var orders = periodicOrderValidations.GroupBy(x => x.ArticleId)
        //                                              .Where(x => x.Count() > 0)
        //                                              .Select(x => new CreateOrdersPost()
        //                                              {
        //                                                  ArticleId = x.Key,
        //                                                  PatientId = x.OrderByDescending(d => d.UpdatedOn).First().PatientId,
        //                                                  Quantity = x.OrderByDescending(d => d.UpdatedOn).First().Quantity
        //                                              })
        //                                              .ToArray();
        //        if (orders.Length > 0)
        //        {
        //            await CreateOrdersAsync(orders, organization.DatabaseName);
        //            var dateTimeNow = _currentDateTime.Now;
        //            foreach (var periodicOrderValidation in periodicOrderValidations)
        //            {
        //                periodicOrderValidation.OrderedOn = dateTimeNow;
        //                await periodicOrderValidationRepository.SaveOrUpdateAsync(periodicOrderValidation);
        //                await repositorySession.Session.FlushAsync();
        //            }
        //            organization.LastPeriodicityOrder = dateTimeNow;
        //            await organizationRepository.SaveOrUpdateAsync(organization);
        //            await repositorySession.Session.FlushAsync();
        //        }
        //    }
        //}

        public async Task UpdateProtectionAsync(UpdateProtectionModel model)
        {
            var query = "exec SP_MDC_UpdateProtection :PrescriptionId, :StartDate, :StopDate, :PosologyExpression, :PosologyJSONDetails, :UserLogin, :UserIp";

            var stockedProcedure = _genericsessionContext.NhibernateSession.CreateSQLQuery(query);

            var currentUserLogin = ((IApplicationPartEntityPropertiesAccessor)this._currentUserService.User.Entity).Login;
            var currentUserIp = GetCurrentIpAddress();

            string posologyExpression = "BLALBLA";
            string posologyJSONDetails = string.Join(",", model.ProtectionDosages);

            stockedProcedure.SetParameter("PrescriptionId ", model.ProtectionId);
            stockedProcedure.SetParameter("StartDate", model.StartDate);
            stockedProcedure.SetParameter("StopDate", model.StopDate);
            stockedProcedure.SetParameter("PosologyExpression", posologyExpression);
            stockedProcedure.SetParameter("PosologyJSONDetails", posologyJSONDetails);
            stockedProcedure.SetParameter("UserLogin", currentUserLogin);
            stockedProcedure.SetParameter("UserIp", currentUserIp);

            await stockedProcedure.ExecuteUpdateAsync();
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