using FwaEu.ElderAid.Stock.Services;
using FwaEu.ElderAid.Treatments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.Treatments.Services
{
    public interface ITreatmentService
    {
        Task<List<GetTreatmentsByPatientResponse>> GetAllTreatmentsByPatientAsync(GetTreatmentsByPatientPost model);

    }
}
