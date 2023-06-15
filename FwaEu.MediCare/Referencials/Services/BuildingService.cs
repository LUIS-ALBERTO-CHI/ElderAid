using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.MediCare.MappingTransformer;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Referencials.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly MainSessionContext _sessionContext;

        public BuildingService(MainSessionContext sessionContext, IMappingTransformer databaseMappingTransformer)
        {
            _sessionContext = sessionContext;
        }
        public async Task<List<BuildingEntity>> GetAllAsync()
        {
            var repository = _sessionContext.RepositorySession;
            return await repository.Create<BuildingEntityRepository>().Query().ToListAsync();
        }
    }
}