using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.Patients.Services
{
    public interface IPatientService
    {
        Task<GetIncontinenceLevel> GetIncontinenceLevelAsync(int id);

        Task SaveIncontinenceLevelAsync(SaveIncontinenceLevel model);
    }
}
