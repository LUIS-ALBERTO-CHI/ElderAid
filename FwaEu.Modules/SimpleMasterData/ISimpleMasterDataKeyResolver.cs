using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.SimpleMasterData
{
	public interface ISimpleMasterDataKeyResolver<TEntity>
		where TEntity : SimpleMasterDataEntityBase
	{
		string ResolveKey();
	}

	public class PluralizationSimpleMasterDataKeyResolver<TEntity> : ISimpleMasterDataKeyResolver<TEntity>
		where TEntity : SimpleMasterDataEntityBase
	{
		private readonly IPluralizationService _pluralizationService;

		public PluralizationSimpleMasterDataKeyResolver(IPluralizationService pluralizationService)
		{
			this._pluralizationService = pluralizationService
				?? throw new ArgumentNullException(nameof(pluralizationService));
		}

		public string ResolveKey()
		{
			var name = FwaConventions.NormalizeEntityName(typeof(TEntity));
			return this._pluralizationService.Pluralize(name);
		}
	}
}
