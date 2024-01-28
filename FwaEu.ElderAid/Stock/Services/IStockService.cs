using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.Stock.Services
{
    public interface IStockService
    {
        Task<List<GetAllStockConsumptionPatientResponse>> GetAllStockConsumptionPatient(GetAllStockConsumptionPatientPost model);

        Task<List<GetAllArticlesCabinetResponse>> GetAllArticlesCabinets(GetAllArticlesCabinetPost model);

        Task UpdateAsync(UpdateStockPost model);
    }
}
