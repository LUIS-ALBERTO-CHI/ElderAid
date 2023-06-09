using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.MediCare.MappingTransformer;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Referencials.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly MainSessionContext _sessionContext;
        private readonly IMappingTransformer _mappingTransformer;

        public BuildingService(MainSessionContext sessionContext, IMappingTransformer databaseMappingTransformer)
        {
            _sessionContext = sessionContext;
            _mappingTransformer = databaseMappingTransformer ?? throw new ArgumentNullException(nameof(databaseMappingTransformer));
        }
        public async Task<List<BuildingEntity>> GetAllAsync()
        {
            var storedProcedureName = "BuildingGetAll";
            var query = "exec " + storedProcedureName;

            var session = (ISession)_sessionContext.RepositorySession.Session.InnerSession;
            return await _mappingTransformer.GetGenericModelAsync<BuildingEntityMappingTransformer, BuildingEntity>(session, query);
        }
    }
}