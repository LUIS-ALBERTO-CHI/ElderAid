using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Referencials.Services
{
    public interface ITreatmentService
    {
        Task<List<TreatmentEntity>> GetAllAsync();

    }
}
