using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Stock.Services
{
    public interface IStockService
    {
        Task<List<GetAllStockConsumptionPatientResponse>> GetAllStockConsumptionPatient(GetAllStockConsumptionPatientPost model);
    }
}
