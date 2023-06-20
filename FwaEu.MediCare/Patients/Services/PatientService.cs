using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.MediCare.GenericRepositorySession;
using NHibernate.Linq;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace FwaEu.MediCare.Patients.Services
{
    public class PatientService : IPatientService
    {
        public PatientService(GenericSessionContext genericSessionContext)
        {
            _genericSessionContext = genericSessionContext;
        }
        private readonly GenericSessionContext _genericSessionContext;

        public async Task<List<PatientEntity>> GetAllAsync()
        {
            var repositorySession = _genericSessionContext.RepositorySession;
            var repositoryy = repositorySession.Create<PatientEntityRepository>();
            return await repositoryy.Query().ToListAsync();
        }
    }
}