using FwaEu.Modules.Authentication.JwtBearerEvents;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.SignalR
{
	public static class SignalRExtensions
	{
		public static void AddFwameworkModuleSignalR(this IServiceCollection services)
		{
			services.AddSignalR();
			services.AddSingleton<IUserIdProvider, UserIdProvider>();
			services.AddTransient<IJwtBearerEventsHandler, SignalRJwtBearerEventsHandler>();
		}
	}
}
