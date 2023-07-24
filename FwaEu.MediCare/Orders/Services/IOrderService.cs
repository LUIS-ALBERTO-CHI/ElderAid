using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Orders.Services
{
    public interface IOrderService
    {
        Task<List<GetAllOrdersResponse>> GetAllAsync(GetAllOrdersPost model);

        Task CreateOrdersAsync(CreateOrdersPost[] orders, bool isPeriodicOrder = false);

        Task ValidatePeriodicOrderAsync(ValidatePeriodicOrderPost validatePeriodicOrder);

        Task CreatePeriodicOrderAsync(int organizationId);
    }
}