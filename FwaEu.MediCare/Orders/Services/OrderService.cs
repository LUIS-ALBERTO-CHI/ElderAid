using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.MediCare.MappingTransformer;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly MainSessionContext _sessionContext;

        public OrderService(MainSessionContext sessionContext, IMappingTransformer databaseMappingTransformer)
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
