using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.MediCare.MappingTransformer;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Referencials.Services
{
    public class TreatmentService : ITreatmentService
    {
        private readonly MainSessionContext _sessionContext;

        public TreatmentService(MainSessionContext sessionContext, IMappingTransformer databaseMappingTransformer)
        {
            _sessionContext = sessionContext;
        }
        public async Task<List<TreatmentEntity>> GetAllAsync()
        {
            var repository = _sessionContext.RepositorySession;
            return await repository.Create<TreatmentEntityRepository>().Query().ToListAsync();
        }
    }
}
