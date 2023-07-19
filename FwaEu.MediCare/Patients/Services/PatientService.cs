using FwaEu.Fwamework.Users;
using FwaEu.MediCare.GenericRepositorySession;
using FwaEu.MediCare.Users;
using Microsoft.AspNetCore.Http;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Patients.Services
{
    public class PatientService : IPatientService
    {
        private readonly GenericSessionContext _sessionContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientService(GenericSessionContext sessionContext, ICurrentUserService currentUserService, IHttpContextAccessor httpContextAccessor)
        {
            _sessionContext = sessionContext;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetIncontinenceLevel> GetIncontinenceLevelAsync(int id)
        {
            var query = "exec SP_MDC_GetIncontinenceLevelByPatientId :PatientId";

            var storedProcedure = _sessionContext.NhibernateSession.CreateSQLQuery(query);
            storedProcedure.SetParameter("PatientId", id);

            var model = await storedProcedure.SetResultTransformer(Transformers.AliasToBean<GetIncontinenceLevel>()).ListAsync<GetIncontinenceLevel>();
            return model.FirstOrDefault();
        }

        public async Task SaveIncontinenceLevelAsync(SaveIncontinenceLevel model)
        {
            if(model.DateStart < model.DateEnd)
                throw new NotImplementedException();
            var query = "exec SP_MDC_SaveIncontinenceLevel :PatientId, :IncontinenceLevel, :StartDateSubscription, :StopDateSubscription, :UserLogin, :UserIp";

            var stockedProcedure = _sessionContext.NhibernateSession.CreateSQLQuery(query);
            var currentUser = (IApplicationPartEntityPropertiesAccessor)this._currentUserService.User.Entity;
            var currentUserIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            stockedProcedure.SetParameter("PatientId", model.Id);
            stockedProcedure.SetParameter("IncontinenceLevel", model.Level);
            stockedProcedure.SetParameter("StartDateSubscription", model.DateStart);
            stockedProcedure.SetParameter("StopDateSubscription", model.DateEnd);
            stockedProcedure.SetParameter("UserLogin", currentUser.Login);
            stockedProcedure.SetParameter("UserIp", currentUserIp);

            await stockedProcedure.ExecuteUpdateAsync();
        }
    }
}