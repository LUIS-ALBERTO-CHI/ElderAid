using FwaEu.Fwamework.Temporal;
using FwaEu.Modules.MasterData;
using System.Threading.Tasks;
using System;
using System.Collections;
using FwaEu.MediCare.Patients.Services;

namespace FwaEu.MediCare.Patients.MasterData
{
    public class PatientMasterDataProvider : IMasterDataProvider
    {
        public PatientMasterDataProvider(ICurrentDateTime currentDateTime, IPatientService patientService)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
            if (!_dateTimeNow.HasValue)
            {
                _dateTimeNow = currentDateTime.Now;
            }
        }

        private readonly IPatientService _patientService;
        public Type IdType => typeof(string);

        private static DateTime? _dateTimeNow = null;

        public async Task<MasterDataChangesInfo> GetChangesInfoAsync(MasterDataProviderGetChangesParameters parameters)
        {
            var count = (await _patientService.GetAllAsync()).Count;

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
            return await _patientService.GetAllAsync();
        }

        public Task<IEnumerable> GetModelsByIdsAsync(MasterDataProviderGetModelsByIdsParameters parameters)
        {
            throw new NotSupportedException(); // NOTE: It's a small master-data, pagination is not useful
        }
    }
}
