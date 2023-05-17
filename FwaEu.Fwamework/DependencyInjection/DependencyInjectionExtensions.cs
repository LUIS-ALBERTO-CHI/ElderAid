using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.DependencyInjection
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddFwameworkDependencyInjection(this IServiceCollection services)
		{
			services.AddSingleton<IScopedServiceProvider, ScopedServiceProvider>();
			services.AddSingleton<AsyncLocalScopedServiceProviderAccessor>();
			services.AddSingleton<IScopedServiceProviderAccessor, AsyncLocalScopedServiceProviderAccessor>();
			services.AddSingleton<IScopedServiceProviderAccessor, HttpContextScopedServiceProviderAccessor>();
			return services;
		}
	}
}
