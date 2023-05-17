using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData
{
	public class DefaultMasterDataService : IMasterDataService
	{
		public DefaultMasterDataService(MasterDataFactoryCache masterDataFactoryCache, IServiceProvider serviceProvider)
		{
			this._masterDataFactoryCache = masterDataFactoryCache
				?? throw new ArgumentNullException(nameof(masterDataFactoryCache));

			this._serviceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		private readonly MasterDataFactoryCache _masterDataFactoryCache;
		private readonly IServiceProvider _serviceProvider;

		private IMasterDataProvider CreateProvider(string key)
		{
			var factory = this._masterDataFactoryCache.GetFactory(key);
			if (factory == null)
			{
				throw new ApplicationException($"No master data factory registered with the key: {key}");

			}
			return factory.Create(this._serviceProvider);
		}

		public Type GetIdType(string key)
		{
			return this.CreateProvider(key).IdType;
		}

		public async Task<RelatedMasterDataChangesInfo[]> GetChangesInfosAsync(
			RelatedParameters<MasterDataProviderGetChangesParameters>[] parameters)
		{
			var tasks = parameters.Select(p =>
				new
				{
					p.Key,
					Task = this.CreateProvider(p.Key)
						.GetChangesInfoAsync(p.Parameters)
				})
				.ToArray();

			//NOTE: Do not run in parallel because NHibernate cannot handle it https://github.com/nhibernate/nhibernate-core/issues/1642
			foreach (var task in tasks)
			{
				await task.Task;
			}

			return tasks.Select(t => new RelatedMasterDataChangesInfo(t.Key, t.Task.Result))
				.ToArray();
		}

		public async Task<RelatedMasterDataGetModelsResult[]> GetModelsAsync(
			RelatedParameters<MasterDataProviderGetModelsParameters>[] parameters)
		{
			var tasks = parameters.Select(p =>
				new
				{
					p.Key,
					Task = this.CreateProvider(p.Key)
						.GetModelsAsync(p.Parameters)
				})
				.ToArray();

			//NOTE: Do not run in parallel because NHibernate cannot handle it https://github.com/nhibernate/nhibernate-core/issues/1642
			foreach (var task in tasks)
			{
				await task.Task;
			}

			return tasks.Select(t => new RelatedMasterDataGetModelsResult(t.Key, t.Task.Result))
				.ToArray();
		}

		public async Task<RelatedMasterDataGetModelsResult[]> GetModelsByIdsAsync(RelatedParameters<MasterDataProviderGetModelsByIdsParameters>[] parameters)
		{
			var tasks = parameters.Select(p =>
				new
				{
					p.Key,
					Task = this.CreateProvider(p.Key)
						.GetModelsByIdsAsync(p.Parameters)
				})
				.ToArray();

			//NOTE: Do not run in parallel because NHibernate cannot handle it https://github.com/nhibernate/nhibernate-core/issues/1642
			foreach (var task in tasks)
			{
				await task.Task;
			}

			return tasks.Select(t => new RelatedMasterDataGetModelsResult(t.Key, t.Task.Result))
				.ToArray();
		}
	}
}
