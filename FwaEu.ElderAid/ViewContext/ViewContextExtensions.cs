using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.ViewContext
{
	public static class ViewContextExtensions
	{
		public static IServiceCollection AddApplicationViewContext(this IServiceCollection services)
		{
			services.AddScoped<IViewContextService, HttpHeaderViewContextService>();

			return services;
		}
	}
}
