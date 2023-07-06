using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Patients.Services
{
    public interface IPatientService
    {
        Task<GetIncontinenceLevel> GetIncontinenceLevelAsync(int id);

        Task SaveIncontinenceLevelAsync(int id, IncontinenceLevel incontinenceLevel);
    }
}
