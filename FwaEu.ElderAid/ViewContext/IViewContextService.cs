using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.ElderAid.GenericRepositorySession;
using FwaEu.ElderAid.GenericSession;
using FwaEu.ElderAid.Organizations;
using FwaEu.ElderAid.Users;
using FwaEu.ElderAid.ViewContext.WebApi;
using Microsoft.AspNetCore.Http;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.ViewContext
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

				try
				{

					if (apiModel != null)
					{

						if (apiModel.Id > 0)
						{
							var currentUser = this._currentUserService.User?.Entity;
							if (currentUser != null)
							{
								var entity = currentUser.IsAdmin
												? (await this._sessionContext.RepositorySession.Create<OrganizationEntityRepository>().GetAsync(apiModel.Id))
												: (await this._sessionContext.RepositorySession.Create<OrganizationUserLinkEntityRepository>().Query()
													.FirstOrDefaultAsync(x => x.User.Id == currentUser.Id && x.Organization.Id == apiModel.Id))?.Organization;
								if (entity != null)
								{
									this.Current = new ViewContextModel(entity.DatabaseName);

									if (!entity.DatabaseName.Equals(_manageGenericDbService.GetGenericDb(), StringComparison.InvariantCultureIgnoreCase))
									{
										_manageGenericDbService.OnChangeGenericDb(entity.Id, entity.DatabaseName);
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
				}
				catch (NotFoundException)
				{
					return ViewContextLoadResult.OutOfPerimeter;
				}
			}

			return ViewContextLoadResult.Loaded; // NOTE: Null is an acceptable value, when there is no header
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