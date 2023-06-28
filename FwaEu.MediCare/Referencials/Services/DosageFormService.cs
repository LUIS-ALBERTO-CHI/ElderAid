using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate.Linq;
using FwaEu.Fwamework.Data.Database.Sessions;

namespace FwaEu.MediCare.Referencials.Services
{
    public class DosageFormService : IDosageFormService
    {
        private readonly MainSessionContext _sessionContext;

        public DosageFormService(MainSessionContext sessionContext)
        {
            _sessionContext = sessionContext;
        }
        public async Task<List<DosageFormEntity>> GetAllAsync()
        {
            var repository = _sessionContext.RepositorySession;
            return await repository.Create<DosageFormEntityRepository>().Query().ToListAsync();
        }
    }
}