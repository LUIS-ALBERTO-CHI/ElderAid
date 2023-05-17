using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Modules.SimpleMasterData.MasterData;
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

		public static IMasterDataProviderInitializer AddMasterDataProvider<TProvider>(this IServiceCollection services, string key)
			where TProvider : class, IMasterDataProvider
		{
			services.AddTransient<IMasterDataProviderFactory>(sp => new InjectedMasterDataProviderFactory<TProvider>(key));
			services.AddTransient<TProvider>();
			var typeEntity = typeof(IEntity);
			var masterdataEntityType = typeof(TProvider).BaseType.GenericTypeArguments.FirstOrDefault(x =>typeEntity.IsAssignableFrom(x));
			var initializer = new MasterDataProviderInitializer(key, services);
			if (masterdataEntityType != null)
			{
				initializer.AddRealtedEntity(masterdataEntityType);
			}
			return initializer;
		}

		
		public static IMasterDataProviderInitializer AddRealtedEntity<TEntity>(this IMasterDataProviderInitializer initializer)
		{
			initializer.AddRealtedEntity(typeof(TEntity));
			return initializer;
		}
		public static IMasterDataProviderInitializer AddRealtedEntity(this IMasterDataProviderInitializer initializer, Type entityType)
		{
			initializer.ServiceCollection.AddTransient<IMasterDataRelatedEntity>(provider =>
			new MasterDataRelatedEntity(initializer.Key, entityType));
			return initializer;
		}
	}
}
