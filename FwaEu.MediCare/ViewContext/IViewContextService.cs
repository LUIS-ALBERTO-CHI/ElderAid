using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.MediCare.GenericRepositorySession;
using FwaEu.MediCare.GenericSession;
using FwaEu.MediCare.Orders.Services;
using FwaEu.MediCare.Organizations;
using FwaEu.MediCare.Users;
using FwaEu.MediCare.ViewContext.WebApi;
using Microsoft.AspNetCore.Http;
using NHibernate.Linq;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.ViewContext
{
    public enum ViewContextLoadResult
    {
        OutOfPerimeter,
        Loaded
    }


    public interface IViewContextService
    {
        ViewContextModel Current { get; }
        Task<ViewContextLoadResult> LoadAsync();
    }

    public class HttpHeaderViewContextService : IViewContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MainSessionContext _sessionContext;
        private readonly GenericSessionContext _sessionContextGeneric;
        private readonly ICurrentUserService _currentUserService;
        private readonly IManageGenericDbService _manageGenericDbService;

        public HttpHeaderViewContextService(
            IHttpContextAccessor httpContextAccessor,
            MainSessionContext sessionContext,
            GenericSessionContext sessionContextGeneric,
            ICurrentUserService currentUserService,
            IManageGenericDbService manageGenericDbService)
        {
            this._httpContextAccessor = httpContextAccessor
                ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this._currentUserService = currentUserService;
            this._sessionContext = sessionContext
                ?? throw new ArgumentNullException(nameof(sessionContext));
            this._sessionContextGeneric = sessionContextGeneric 
                ?? throw new ArgumentNullException(nameof(sessionContext));
            _manageGenericDbService = manageGenericDbService;
        }

        public ViewContextModel Current { get; private set; }

        private static ViewContextApiModel Deserialize(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ViewContextApiModel>(value);
        }

        public async Task<ViewContextLoadResult> LoadAsync()
        {
            var headerValue = this._httpContextAccessor.HttpContext.Request.Headers["View-Context"].FirstOrDefault();
            if (!String.IsNullOrEmpty(headerValue))
            {
                var apiModel = Deserialize(headerValue);

                if (apiModel != null)
                {
                    if (apiModel.Id > 0)
                    {
                        var currentUser = this._currentUserService.User.Entity;
                        var entity = currentUser.IsAdmin 
                                        ? (await this._sessionContext.RepositorySession.Create<OrganizationEntityRepository>().GetAsync(apiModel.Id))
                                        : (await this._sessionContext.RepositorySession.Create<OrganizationUserLinkEntityRepository>().Query()
                                            .FirstOrDefaultAsync(x => x.User.Id == currentUser.Id  && x.Organization.Id == apiModel.Id))?.Organization;
                        if (entity != null)
                        {
                            this.Current = new ViewContextModel(entity.DatabaseName); 
                            
                            if (!entity.DatabaseName.Equals(_manageGenericDbService.GetGenericDb(), StringComparison.InvariantCultureIgnoreCase))
                            {
                                _manageGenericDbService.OnChangeGenericDb(entity.DatabaseName);
                                await AddLogUserAsync((IApplicationPartEntityPropertiesAccessor)currentUser);
                            }
                        }
                        else
                        {
                            return ViewContextLoadResult.OutOfPerimeter;
                        }

                    }
                }
            }
            return ViewContextLoadResult.Loaded;
        }


        private async Task AddLogUserAsync(IApplicationPartEntityPropertiesAccessor currentUser)
        {
            var query = "exec SP_MDC_AddUserLogDb :UserLogin, :UserIp";

            var stockedProcedure = _sessionContextGeneric.NhibernateSession.CreateSQLQuery(query);
            var currentUserIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            stockedProcedure.SetParameter("UserLogin", currentUser.Login);
            stockedProcedure.SetParameter("UserIp", currentUserIp);
            await stockedProcedure.ExecuteUpdateAsync();
        }
    }
}