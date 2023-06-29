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

namespace FwaEu.MediCare.Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly GenericSessionContext _sessionContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(GenericSessionContext sessionContext, ICurrentUserService currentUserService, IHttpContextAccessor httpContextAccessor)
        {
            _sessionContext = sessionContext;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<GetAllOrdersResponse>> GetAllAsync(GetAllOrdersPost model)
        {
            var query = "exec SP_MDC_Orders :Page, :PageSize, :PatientId";

            var storedProcedure = _sessionContext.NhibernateSession.CreateSQLQuery(query);
            storedProcedure.SetParameter("PatientId", model.PatientId);
            storedProcedure.SetParameter("Page", model.Page);
            storedProcedure.SetParameter("PageSize", model.PageSize);

            var models = await storedProcedure.SetResultTransformer(Transformers.AliasToBean<GetAllOrdersResponse>()).ListAsync<GetAllOrdersResponse>();
            return models.ToList();
        }

        public async Task CreateOrdersAsync(CreateOrdersPost[] orders)
        {
            var query = "exec SP_MDC_AddOrder :PatientId, :ArticleId, :Quantity, :UserLogin, :UserIp";

            var stockedProcedure = _sessionContext.NhibernateSession.CreateSQLQuery(query);
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
    }
}