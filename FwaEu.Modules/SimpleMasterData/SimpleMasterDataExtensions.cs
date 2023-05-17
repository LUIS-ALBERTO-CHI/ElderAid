using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.SimpleMasterData
{
	public static class SimpleMasterDataExtensions
	{
		public static ConstructSimpleMasterDataServiceInitializer<TEntity>
			For<TEntity>(this IServiceCollection serviceCollection, ApplicationInitializationContext context)
				where TEntity : SimpleMasterDataEntityBase
		{
			
			var state = context.ServiceStore.GetOrAdd<SimpleMasterDataInitializationState>();

			if (!state.DefaultKeyResolverInitialized)
			{
				serviceCollection.AddTransient(typeof(ISimpleMasterDataKeyResolver<>), typeof(PluralizationSimpleMasterDataKeyResolver<>));
				state.DefaultKeyResolverInitialized = true;
			}

			return new ConstructSimpleMasterDataServiceInitializer<TEntity>(serviceCollection, context);
		}
	}

	public class SimpleMasterDataInitializationState
	{
		public bool DefaultKeyResolverInitialized { get; set; } = false;
	}
}
