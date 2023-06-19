using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.MediCare.GenericRepositorySession;

using NHibernate.Linq;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace FwaEu.MediCare.Referencials.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly IGenericRepositorySessionService _genericRepositorySessionService;

        public BuildingService(IGenericRepositorySessionService genericRepositorySessionService)
        {
            _genericRepositorySessionService = genericRepositorySessionService;
        }

        public async Task<List<BuildingEntity>> GetAllAsync()
        {
            var repositorySession = _genericRepositorySessionService.GetRepositorySession();
            var repositoryy = repositorySession.Create<BuildingEntityRepository>();
            return await repositoryy.Query().ToListAsync();
        }
    }
}