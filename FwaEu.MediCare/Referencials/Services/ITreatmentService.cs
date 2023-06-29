using FwaEu.MediCare.Stock.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Referencials.Services
{
    public interface ITreatmentService
    {
        Task<List<TreatmentEntity>> GetAllAsync();
        Task<List<GetTreatmentsByPatientResponse>> GetAllTreatmentsByPatientAsync(GetTreatmentsByPatientPost model);

    }
}
