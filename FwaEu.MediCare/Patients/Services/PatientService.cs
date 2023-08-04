using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.MediCare.GenericRepositorySession;
using FwaEu.MediCare.Orders;
using FwaEu.MediCare.Organizations;
using FwaEu.MediCare.Referencials;
using FwaEu.MediCare.Users;
using Microsoft.AspNetCore.Http;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FwaEu.MediCare.Patients.Services
{
    public class PatientService : IPatientService
    {
        private readonly GenericSessionContext _sessionContext;
        private readonly MainSessionContext _mainSessionContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientService(GenericSessionContext sessionContext, MainSessionContext mainSessionContext, ICurrentUserService currentUserService, IHttpContextAccessor httpContextAccessor)
        {
            _sessionContext = sessionContext;
            _mainSessionContext = mainSessionContext;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetIncontinenceLevel> GetIncontinenceLevelAsync(int id)
        {
            var query = "exec SP_MDC_GetIncontinenceLevelByPatientId :PatientId";
            var storedProcedure = _sessionContext.NhibernateSession.CreateSQLQuery(query);
            storedProcedure.SetParameter("PatientId", id);

            var incontinenceLevelModel = (await storedProcedure.SetResultTransformer(Transformers.AliasToBean<GetIncontinenceLevel>()).ListAsync<GetIncontinenceLevel>()).FirstOrDefault();

            if (incontinenceLevelModel != null)
            {
                int currentYear = DateTime.Now.Year;
                var repository = _mainSessionContext.RepositorySession.Create<IncontinenceLevelEntityRepository>();
                var IncontinenceLevelEMS = await repository.Query().FirstOrDefaultAsync(x => x.Year == currentYear &&
                                                                             (int)x.Level == incontinenceLevelModel.IncontinenceLevel);
                int totalDaysInYear = (new DateTime(currentYear, 12, 31) - new DateTime(currentYear, 1, 1)).Days;
                if (IncontinenceLevelEMS != null)
                {
                    incontinenceLevelModel.AnnualFixedPrice = IncontinenceLevelEMS.Amount;
                    incontinenceLevelModel.DailyFixedPrice = incontinenceLevelModel.AnnualFixedPrice / totalDaysInYear;
                }
                int totalDaysFromFirstDayInYearToDateNow = (DateTime.Now - new DateTime(currentYear, 1, 1)).Days;
                incontinenceLevelModel.FixedPrice = totalDaysFromFirstDayInYearToDateNow * incontinenceLevelModel.DailyFixedPrice;
                incontinenceLevelModel.OverPassed = incontinenceLevelModel.FixedPrice - incontinenceLevelModel.Consumed;
                incontinenceLevelModel.DailyProtocolEntered = incontinenceLevelModel.Consumed / totalDaysFromFirstDayInYearToDateNow;

                // NOTE: We can remove this comment later
                //incontinenceLevelModel.FixedPrice = 365 - 148;
                //incontinenceLevelModel.AnnualFixedPrice = 365;
                //incontinenceLevelModel.DailyFixedPrice = 1;
                //incontinenceLevelModel.Consumed = 148;
                //incontinenceLevelModel.OverPassed = incontinenceLevelModel.FixedPrice - incontinenceLevelModel.Consumed;
                
                var virtualDateWithoutOverPassed = incontinenceLevelModel.OverPassed  / (incontinenceLevelModel.FixedPrice / totalDaysFromFirstDayInYearToDateNow);
                incontinenceLevelModel.VirtualDateWithoutOverPassed = (new DateTime(currentYear, 12, 31)).AddDays(virtualDateWithoutOverPassed);
            }
            return incontinenceLevelModel;
        }

        public async Task SaveIncontinenceLevelAsync(SaveIncontinenceLevel model)
        {
            if(model.DateStart > model.DateEnd)
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