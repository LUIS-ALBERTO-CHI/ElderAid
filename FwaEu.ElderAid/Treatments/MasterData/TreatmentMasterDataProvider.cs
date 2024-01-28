using FwaEu.Fwamework.Temporal;
using FwaEu.Modules.MasterData;
using System.Threading.Tasks;
using System;
using System.Collections;
using FwaEu.ElderAid.GenericRepositorySession;
using System.Collections.Generic;
using FwaEu.Fwamework.Data.Database.Sessions;
using NHibernate.Linq;
using System.Linq;

namespace FwaEu.ElderAid.Treatments.MasterData
{
    public class TreatmentMasterDataProvider : IMasterDataProvider
    {
        public TreatmentMasterDataProvider(ICurrentDateTime currentDateTime, GenericSessionContext sessionContext)
        {
            _sessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));
            if (!_dateTimeNow.HasValue)
            {
                _dateTimeNow = currentDateTime.Now;
            }
        }

        private readonly GenericSessionContext _sessionContext;
        public Type IdType => typeof(string);

        private static DateTime? _dateTimeNow = null;

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

        public Task<IEnumerable> GetModelsByIdsAsync(MasterDataProviderGetModelsByIdsParameters parameters)
        {
            throw new NotSupportedException(); // NOTE: It's a small master-data, pagination is not useful
        }

        protected async Task<IEnumerable<TreatmentEntity>> GetAllAsync()
        {
            return await _sessionContext.RepositorySession.Create<TreatmentEntityRepository>().Query().ToListAsync();
        }

    }

}
