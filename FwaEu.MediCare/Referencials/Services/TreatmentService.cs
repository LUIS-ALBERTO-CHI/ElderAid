using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.MediCare.GenericRepositorySession;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Referencials.Services
{
    public class TreatmentService : ITreatmentService
    {
        private readonly GenericSessionContext _sessionContext;

        public TreatmentService(GenericSessionContext sessionContext)
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