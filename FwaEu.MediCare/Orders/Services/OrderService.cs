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
using FwaEu.Fwamework.Data.Database.Sessions;
using NHibernate.Exceptions;
using FwaEu.Modules.Data.Database;
using FwaEu.Fwamework.Temporal;
using FwaEu.MediCare.Organizations;
using FwaEu.MediCare.GenericSession;

namespace FwaEu.MediCare.Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly GenericSessionContext _genericsessionContext;
        private readonly MainSessionContext _mainSessionContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentDateTime _currentDateTime;
        private readonly IManageGenericDbService _manageGenericDbService;

        public OrderService(GenericSessionContext genericSessionContext, MainSessionContext mainSessionContext, ICurrentUserService currentUserService,
                            IHttpContextAccessor httpContextAccessor, ICurrentDateTime currentDateTime, IManageGenericDbService manageGenericDbService)
        {
            _genericsessionContext = genericSessionContext;
            _mainSessionContext = mainSessionContext;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
            _currentDateTime = currentDateTime;
            _manageGenericDbService = manageGenericDbService;
        }

        public async Task<List<GetAllOrdersResponse>> GetAllAsync(GetAllOrdersPost model)
        {
            var query = "exec SP_MDC_Orders :Page, :PageSize, :PatientId";

            var storedProcedure = _genericsessionContext.NhibernateSession.CreateSQLQuery(query);
            storedProcedure.SetParameter("PatientId", model.PatientId);
            storedProcedure.SetParameter("Page", model.Page);
            storedProcedure.SetParameter("PageSize", model.PageSize);

            var models = await storedProcedure.SetResultTransformer(Transformers.AliasToBean<GetAllOrdersResponse>()).ListAsync<GetAllOrdersResponse>();
            return models.ToList();
        }

        public async Task CreateOrdersAsync(CreateOrdersPost[] orders)
        {
            var query = "exec SP_MDC_AddOrder :PatientId, :ArticleId, :Quantity, :UserLogin, :UserIp";

            var stockedProcedure = _genericsessionContext.NhibernateSession.CreateSQLQuery(query);
            var currentUser = (IApplicationPartEntityPropertiesAccessor)this._currentUserService.User.Entity;
            var currentUserIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            foreach (var order in orders)
            {

                stockedProcedure.SetParameter("PatientId", order.PatientId);
                stockedProcedure.SetParameter("ArticleId", order.ArticleId);
                stockedProcedure.SetParameter("Quantity", order.Quantity);
                stockedProcedure.SetParameter("UserLogin", currentUser.Login);
                stockedProcedure.SetParameter("UserIp", currentUserIp);

                await stockedProcedure.ExecuteUpdateAsync();
            }
        }

        public async Task ValidatePeriodicOrderAsync(ValidatePeriodicOrderPost validatePeriodicOrder)
        {
            try
            {
                var repositorySession = this._mainSessionContext.RepositorySession;
                var repository = repositorySession.Create<PeriodicOrderValidationEntityRepository>();
                PeriodicOrderValidationEntity entity;

                var organizationRepository = repositorySession.Create<OrganizationEntityRepository>();
                var currentDbId = _manageGenericDbService.GetGenericDbId();
                var organization = await organizationRepository.GetAsync(1);

                var currentUser = this._currentUserService.User.Entity;
                var dateNow = _currentDateTime.Now;
                foreach (var article in validatePeriodicOrder.Articles)
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
                    await repository.SaveOrUpdateAsync(entity);
                    await repositorySession.Session.FlushAsync();
                }
            }
            catch (GenericADOException e)
            {
                DatabaseExceptionHelper.CheckForDbConstraints(e);
                throw;
            }
        }
    }
}