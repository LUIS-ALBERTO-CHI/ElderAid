using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.MediCare.Organizations;
using FwaEu.MediCare.ViewContext.WebApi;
using Microsoft.AspNetCore.Http;
using NHibernate.Linq;
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

		public HttpHeaderViewContextService(
			IHttpContextAccessor httpContextAccessor,
			MainSessionContext sessionContext)
		{
			this._httpContextAccessor = httpContextAccessor
				?? throw new ArgumentNullException(nameof(httpContextAccessor));

			this._sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));
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
					if (apiModel.DatabaseName != null)
					{
                        var entity = await this._sessionContext.RepositorySession.Create<OrganizationUserLinkEntityRepository>().Query()
									.FirstOrDefaultAsync( x => x.Organization.DatabaseName.Equals(apiModel.DatabaseName, StringComparison.InvariantCultureIgnoreCase));
						if (entity != null)
						{
							this.Current = new ViewContextModel(entity.Organization.DatabaseName);
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
	}
}