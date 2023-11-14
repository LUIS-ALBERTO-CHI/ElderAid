using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.MediCare.GenericRepositorySession;
using FwaEu.MediCare.Orders;
using FwaEu.MediCare.Organizations;
using FwaEu.MediCare.Referencials;
using FwaEu.MediCare.Referencials.GenericAdmin;
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


        private class GetIncontinenceLevelProcedureModel
        {
            public int Id { get; set; }
            public int IncontinenceLevel { get; set; }
            public double Consumed { get; set; }
            public DateTime DateStart { get; set; }
            public DateTime DateEnd { get; set; }
        }

        public async Task<GetIncontinenceLevel> GetIncontinenceLevelAsync(int id)
        {
            var query = "exec SP_MDC_GetIncontinenceLevelByPatientId :PatientId";
            var storedProcedure = _sessionContext.NhibernateSession.CreateSQLQuery(query);
            storedProcedure.SetParameter("PatientId", id);

            var result = (await storedProcedure.SetResultTransformer(Transformers.AliasToBean<GetIncontinenceLevelProcedureModel>()).ListAsync<GetIncontinenceLevelProcedureModel>()).FirstOrDefault();

            if (result != null)
            {
                int currentYear = DateTime.Now.Year;
                var repository = _mainSessionContext.RepositorySession.Create<IncontinenceLevelEntityRepository>();
                var incontinenceLevelEMS = await repository.Query().FirstOrDefaultAsync(x => x.Year == currentYear &&
                                                                             (int)x.Level == result.IncontinenceLevel);

                int totalDaysInYear = (new DateTime(currentYear, 12, 31) - new DateTime(currentYear, 1, 1)).Days;
                int totalDaysFromStartDateToEndDate = (result.DateEnd - result.DateStart).Days;
                var annualFixedPrice = incontinenceLevelEMS?.Amount ?? 0;
                var dailyFixedPrice = annualFixedPrice / totalDaysInYear;
                var fixedPrice = totalDaysFromStartDateToEndDate * dailyFixedPrice;
                var overPassed = result.Consumed < fixedPrice ? 0 : result.Consumed - fixedPrice;
                var virtualDateWithoutOverPassed = fixedPrice == 0 || totalDaysFromStartDateToEndDate == 0 ? 0
                    : overPassed / (fixedPrice / totalDaysFromStartDateToEndDate);

                var incontinenceLevelModel = new GetIncontinenceLevel
                {
                    Id = result.Id,
                    DateEnd = result.DateEnd,
                    DateStart = result.DateStart,
                    IncontinenceLevel = result.IncontinenceLevel,
                    AnnualFixedPrice = annualFixedPrice,
                    DailyFixedPrice = annualFixedPrice / totalDaysInYear,
                    Consumed = result.Consumed,
                    FixedPrice = fixedPrice,
                    OverPassed = overPassed,
                    DailyProtocolEntered = result.Consumed / totalDaysFromStartDateToEndDate,
                    VirtualDateWithoutOverPassed = (new DateTime(currentYear, 12, 31)).AddDays(virtualDateWithoutOverPassed)
                };
                return incontinenceLevelModel;
            }
            return null;
        }

        public async Task SaveIncontinenceLevelAsync(SaveIncontinenceLevel model)
        {
            if (model.DateStart > model.DateEnd)
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