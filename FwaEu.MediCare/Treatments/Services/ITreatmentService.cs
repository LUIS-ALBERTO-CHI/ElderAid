using FwaEu.MediCare.Stock.Services;
using FwaEu.MediCare.Treatments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Treatments.Services
{
    public interface ITreatmentService
    {
        Task<List<GetTreatmentsByPatientResponse>> GetAllTreatmentsByPatientAsync(GetTreatmentsByPatientPost model);

    }
}
