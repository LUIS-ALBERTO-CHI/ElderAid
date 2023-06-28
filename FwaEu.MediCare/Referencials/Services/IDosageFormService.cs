using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Referencials.Services
{
    public interface IDosageFormService
    {
        Task<List<DosageFormEntity>> GetAllAsync();
        
    }
}