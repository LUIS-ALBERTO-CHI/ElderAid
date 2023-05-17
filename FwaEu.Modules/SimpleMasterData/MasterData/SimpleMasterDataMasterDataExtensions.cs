using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.SimpleMasterData.MasterData
{
	public static class SimpleMasterDataMasterDataExtensions
	{
		public static ISimpleMasterDataServiceInitializer AddCustomMasterDataProviderFactory<TProvider>(
			this ISimpleMasterDataServiceInitializer initializer)
			where TProvider : class, IMasterDataProvider
		{
			var type = typeof(SimpleMasterDataProviderFactory<,>)
				.MakeGenericType(initializer.EntityType, typeof(TProvider));

			initializer.ServiceCollection.AddTransient(typeof(IMasterDataProviderFactory), type);
			initializer.ServiceCollection.AddTransient<TProvider>();

			return initializer;
		}

		public static ISimpleMasterDataServiceInitializer AddDefaultCultureMasterDataProviderFactory(
			this ISimpleMasterDataServiceInitializer initializer)
		{
			var type = typeof(DefaultCultureSimpleMasterDataEntityProvider<,>)
				.MakeGenericType(initializer.RepositoryType, initializer.EntityType);

			initializer.InvokeMethod(typeof(SimpleMasterDataMasterDataExtensions),
				nameof(AddCustomMasterDataProviderFactory), new[] { type });

			return initializer;
		}

		public static ISimpleMasterDataServiceInitializer AddMasterDataProviderFactory(
			this ISimpleMasterDataServiceInitializer initializer)
		{
			var type = typeof(SimpleMasterDataEntityProvider<,>)
				.MakeGenericType(initializer.RepositoryType, initializer.EntityType);

			initializer.InvokeMethod(typeof(SimpleMasterDataMasterDataExtensions),
				nameof(AddCustomMasterDataProviderFactory), new[] { type });

			return initializer;
		}
	}
}
