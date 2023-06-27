using FwaEu.Fwamework.Temporal;
using FwaEu.MediCare.Referencials.Services;
using FwaEu.Modules.MasterData;
using System.Threading.Tasks;
using System;
using System.Collections;

namespace FwaEu.MediCare.Referencials.MasterData
{
    public class TreatmentMasterDataProvider : IMasterDataProvider
    {
        public TreatmentMasterDataProvider(ICurrentDateTime currentDateTime, ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService ?? throw new ArgumentNullException(nameof(treatmentService));
            if (!_dateTimeNow.HasValue)
            {
                _dateTimeNow = currentDateTime.Now;
            }
        }

        private readonly ITreatmentService _treatmentService;
        public Type IdType => typeof(string);

        private static DateTime? _dateTimeNow = null;

        public async Task<MasterDataChangesInfo> GetChangesInfoAsync(MasterDataProviderGetChangesParameters parameters)
        {
            var count = (await _treatmentService.GetAllAsync()).Count;

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
            return await _treatmentService.GetAllAsync();
        }

        public Task<IEnumerable> GetModelsByIdsAsync(MasterDataProviderGetModelsByIdsParameters parameters)
        {
            throw new NotSupportedException(); // NOTE: It's a small master-data, pagination is not useful
        }
    }
}
