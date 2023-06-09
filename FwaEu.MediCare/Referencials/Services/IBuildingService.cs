using System.Collections.Generic;
using System.Threading.Tasks;
using FwaEu.MediCare.Referencials;

namespace FwaEu.MediCare.Referencials.Services
{
    public interface IBuildingService
    {
        Task<List<BuildingEntity>> GetAllAsync();
    }
}
