using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.MediCare.MappingTransformer;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Patients.Services
{
    public class PatientService : IPatientService
    {
        private readonly MainSessionContext _sessionContext;

        public PatientService(MainSessionContext sessionContext, IMappingTransformer databaseMappingTransformer)
        {
            _sessionContext = sessionContext;
        }
        public async Task<List<PatientEntity>> GetAllAsync()
        {
            var repository = _sessionContext.RepositorySession;
            return await repository.Create<PatientEntityRepository>().Query().ToListAsync();
        }
    }
}