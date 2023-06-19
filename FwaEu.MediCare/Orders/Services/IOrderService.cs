using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Orders.Services
{
    public interface IOrderService
    {
        Task<List<OrderEntity>> GetAllAsync();
    }
}
