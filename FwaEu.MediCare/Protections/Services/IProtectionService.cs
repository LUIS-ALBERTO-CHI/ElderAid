using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Protections.Services
{
    public interface IProtectionService
    {
        //Task<List<GetAllOrdersResponse>> GetAllAsync(GetAllOrdersPost model);

        //Task CreateOrdersAsync(CreateOrdersPost[] orders, string databaseName = null);

        //Task ValidatePeriodicOrderAsync(ValidatePeriodicOrderPost validatePeriodicOrder);

        //Task CreatePeriodicOrderAsync(int organizationId);
        Task UpdateProtectionAsync(UpdateProtectionModel model);
        Task StopProtectionAsync(StopProtectionModel model);
    }
}