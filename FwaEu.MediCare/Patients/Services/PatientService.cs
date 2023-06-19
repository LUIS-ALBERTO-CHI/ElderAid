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
        private readonly IGenericRepositorySessionService _genericRepositorySessionService;

        public PatientService(IGenericRepositorySessionService genericRepositorySessionService)
        {
            _genericRepositorySessionService = genericRepositorySessionService;
        }

        public async Task<List<PatientEntity>> GetAllAsync()
        {
            var repositorySession = _genericRepositorySessionService.GetRepositorySession();
            var repositoryy = repositorySession.Create<PatientEntityRepository>();
            return await repositoryy.Query().ToListAsync();
        }
    }
}