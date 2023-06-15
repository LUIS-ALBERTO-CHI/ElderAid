using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Patients.Services
{
    public interface IPatientService
    {
        Task<List<PatientEntity>> GetAllAsync();
    }
}
