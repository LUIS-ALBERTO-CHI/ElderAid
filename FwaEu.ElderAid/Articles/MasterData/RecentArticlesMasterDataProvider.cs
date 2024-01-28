using FwaEu.Fwamework.Temporal;
using FwaEu.Modules.MasterData;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using FwaEu.ElderAid.GenericRepositorySession;
using FwaEu.Fwamework.Data.Database.Sessions;
using NHibernate.Linq;
using FwaEu.ElderAid.Referencials;

namespace FwaEu.ElderAid.Articles.MasterData
{
    public class RecentArticlesMasterDataProvider : IMasterDataProvider
    {
        public RecentArticlesMasterDataProvider(ICurrentDateTime currentDateTime, GenericSessionContext sessionContext, MainSessionContext mainSessionContext)
        {
            _sessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));
            if (!_dateTimeNow.HasValue)
            {
                _dateTimeNow = currentDateTime.Now;
            }
            if (GalenicDosageForms == null)
            {
                GalenicDosageForms = mainSessionContext.RepositorySession.Create<DosageFormEntityRepository>().Query().Select(x => x.Name).ToArray();
            }
        }

        private readonly GenericSessionContext _sessionContext;
        public Type IdType => typeof(string);

        private static DateTime? _dateTimeNow = null;
        public static string[] GalenicDosageForms = null;

        public async Task<MasterDataChangesInfo> GetChangesInfoAsync(MasterDataProviderGetChangesParameters parameters)
        {
            var count = (await GetAllAsync()).Count();

            return await Task.FromResult(new MasterDataChangesInfo(_dateTimeNow, count));
        }

        public async Task<IEnumerable> GetModelsAsync(MasterDataProviderGetModelsParameters parameters)
        {
            if (parameters.Search != null)
            {
                throw new NotSupportedException("Search is not supported by building master-data.");
            }

            if (parameters.Pagination != null)
            {
                throw new NotSupportedException("Pagination is not supported by building master-data.");
            }

            if (parameters.OrderBy != null)
            {
                throw new NotSupportedException("OrderBy is not supported by building master-data.");
            }
            return await GetAllAsync();
        }

        public async Task<IEnumerable> GetModelsByIdsAsync(MasterDataProviderGetModelsByIdsParameters parameters)
        {
            int[] articleIds = parameters.Ids.Select(Convert.ToInt32).ToArray();
            var models = await GetAllAsync();
  
            return models.Where(x => articleIds.Contains(x.Id));
        }


        protected async Task<IEnumerable<ArticleEntity>> GetAllAsync()
        {
            var models = await _sessionContext.RepositorySession.Create<ArticleEntityRepository>().Query().ToListAsync();
            foreach (var model in models)
            {
                model.IsGalenicDosageForm = GalenicDosageForms.Contains(model.Unit, StringComparer.OrdinalIgnoreCase);
            }
            return models;
        }
    }
}
