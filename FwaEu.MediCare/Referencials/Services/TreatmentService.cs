using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.MediCare.GenericRepositorySession;
using NHibernate.Linq;
using NHibernate.Transform;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<GetTreatmentsByPatientResponse>> GetAllTreatmentsByPatientAsync(GetTreatmentsByPatientPost model)
        {
            var query = "exec SP_MDC_TreatmentsPatient :PatientId, :State, :ArticleFamily, :Page, :PageSize";

            var storedProcedure = _sessionContext.NhibernateSession.CreateSQLQuery(query);
            storedProcedure.SetParameter("PatientId", model.PatientId);
            storedProcedure.SetParameter("State", model.State);
            storedProcedure.SetParameter("ArticleFamily", model.ArticleFamily);
            storedProcedure.SetParameter("Page", model.Page);
            storedProcedure.SetParameter("PageSize", model.PageSize);

            var models = await storedProcedure.SetResultTransformer(Transformers.AliasToBean<GetTreatmentsByPatientResponse>()).ListAsync<GetTreatmentsByPatientResponse>();
            return models.ToList();
        }
    }
}