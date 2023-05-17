using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.GenericAdmin;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.SimpleMasterData.GenericAdmin
{
	public static class SimpleMasterDataGenericAdminExtensions
	{
		public static ISimpleMasterDataServiceInitializer AddCustomGenericAdminModelConfiguration<TGenericAdminConfiguration>(
			this ISimpleMasterDataServiceInitializer initializer,
			Action<IGenericAdminSimpleMasterDataServiceInitializer> options = null)
				where TGenericAdminConfiguration : class, IGenericAdminModelConfiguration
		{
			initializer.ServiceCollection.AddTransient<IGenericAdminModelConfiguration, TGenericAdminConfiguration>();
			options?.Invoke(new GenericAdminSimpleMasterDataServiceInitializer(initializer));

			return initializer;
		}

		public static ISimpleMasterDataServiceInitializer AddGenericAdminModelConfiguration(
			this ISimpleMasterDataServiceInitializer initializer,
			Action<IGenericAdminSimpleMasterDataServiceInitializer> options = null)
		{
			var type = typeof(SimpleMasterDataEntityToModelGenericAdminModelConfiguration<,>)
				.MakeGenericType(initializer.EntityType, typeof(SimpleMasterDataGenericAdminModel));

			initializer.InvokeMethod(typeof(SimpleMasterDataGenericAdminExtensions),
				nameof(AddCustomGenericAdminModelConfiguration),
				new[] { type }, options);

			return initializer;
		}
	}
}
