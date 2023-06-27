using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.MediCare.GenericRepositorySession;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly GenericSessionContext _sessionContext;

        public OrderService(GenericSessionContext sessionContext)
        {
            _sessionContext = sessionContext;
        }
        public async Task<List<OrderEntity>> GetAllAsync()
        {
            var repository = _sessionContext.RepositorySession;
            return await repository.Create<OrderEntityRepository>().Query().ToListAsync();
        }
    }
}
