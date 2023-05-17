using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData
{
	public static class MasterDataExtensions
	{
		public static IServiceCollection AddFwameworkModuleMasterDataServices(this IServiceCollection services)
		{
			services.AddSingleton<MasterDataFactoryCache>();
			services.AddTransient<IMasterDataService, DefaultMasterDataService>();

			return services;
		}

		public static IServiceCollection AddMasterDataProvider<TProvider>(this IServiceCollection services, string key)
			where TProvider : class, IMasterDataProvider
		{
			services.AddTransient<IMasterDataProviderFactory>(sp => new InjectedMasterDataProviderFactory<TProvider>(key));
			services.AddTransient<TProvider>();

			return services;
		}
	}
}
