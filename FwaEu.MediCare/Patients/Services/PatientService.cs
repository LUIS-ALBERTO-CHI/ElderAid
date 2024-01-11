using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.DependencyInjection;
using FwaEu.Fwamework.Temporal;
using FwaEu.Fwamework.Users;
using FwaEu.MediCare.Articles.Services;
using FwaEu.MediCare.GenericRepositorySession;
using FwaEu.MediCare.Protections;
using FwaEu.MediCare.Referencials;
using FwaEu.MediCare.Users;
using FwaEu.Modules.BackgroundTasks;
using FwaEu.Modules.MasterData;
using FwaEu.Modules.UserNotifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        private readonly MainSessionContext _mainSessionContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IArticleService _articleService;
        private readonly ILogger<BackgroundTasksBackgroundService> _logger;
		private readonly IScopedServiceProvider _scopedServiceProvider;

		public PatientService(GenericSessionContext sessionContext,
			IScopedServiceProvider scopedServiceProvider,
			MainSessionContext mainSessionContext,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor,
            IArticleService articleService,
            ILogger<BackgroundTasksBackgroundService> logger)
        {
            _sessionContext = sessionContext;
            _mainSessionContext = mainSessionContext;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
            _articleService = articleService;
            _logger = logger;
			this._scopedServiceProvider = scopedServiceProvider
			?? throw new ArgumentNullException(nameof(scopedServiceProvider));
		}


		private class GetIncontinenceLevelProcedureModel
        {
            public int Id { get; set; }
            public int IncontinenceLevel { get; set; }
            public double Consumed { get; set; }
			public double DailyProtocolEntered { get; set; }

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
                double dailyProtocolConsumed = await CalculDailyProtocolConsumed(id);

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

                if (result.DateStart < DateTime.Now && result.Consumed == 0)
                {
                    _logger.LogWarning($"TO CHECK: Patient id {id} has no consume but the start date is {result.DateStart.ToString()} ");
                }

                //date virtuelle de dépassement
                //date du jour + (forfait à date du jour -dépense à date du jour) / (dépenses de protocole journalier - forfait du jour)

                DateTime? virtualDateWithoutOverPassedFinal = null;

                if (overPassed == 0)
                {
                    var dailyProtocolEntered = result.Consumed / totalDaysFromStartDateToEndDate;

                    var virtualDateWithoutOverPassed = (dailyFixedPrice - result.Consumed) / (dailyProtocolConsumed - dailyFixedPrice);
                    virtualDateWithoutOverPassedFinal = DateTime.Now.AddDays(virtualDateWithoutOverPassed);
                }

                var incontinenceLevelModel = new GetIncontinenceLevel
                {
                    Id = result.Id,
                    DateEnd = result.DateEnd,
                    DateStart = result.DateStart,
                    IncontinenceLevel = result.IncontinenceLevel,
                    AnnualFixedPrice = annualFixedPrice,
                    DailyFixedPrice = dailyFixedPrice,
                    Consumed = result.Consumed,
                    FixedPrice = fixedPrice,
					OverPassed = overPassed,
                    DailyProtocolEntered = result.DailyProtocolEntered,
                    VirtualDateWithoutOverPassed = virtualDateWithoutOverPassedFinal
                };
                return incontinenceLevelModel;
            }
            return null;
        }

        private async Task<double> CalculDailyProtocolConsumed(int id)
        {
            var protectionRepository = _sessionContext.RepositorySession.Create<ProtectionEntityRepository>();
            var protections = await protectionRepository.QueryNoPerimeter().Where(x => x.PatientId == id).ToListAsync();
            var articleIds = protections.Select(x => x.ArticleId).ToArray();
            var articles = await _articleService.GetAllByIdsAsync(articleIds);
            var articlesGroup = articles.GroupBy(x => x.Id)
                               .Select(x => new
                               {
                                   x.First().Id,
                                   x.First().Price,
                                   x.First().CountInBox
                               });
            double protocoleJournalierArticleComsumed = 0;

            foreach (var article in articlesGroup)
            {
                if (!article.Price.HasValue)
                {
                    _logger.LogWarning($"Article id {article.Id} has no price ");
                }
                var proctection = protections.FirstOrDefault(x => x.ArticleId == article.Id);
                var proctectionQuantity = proctection.QuantityPerDay;
                var protocoleJournalierArticle = (article.Price.HasValue && article.CountInBox.HasValue) ? proctectionQuantity * (article.Price.Value / article.CountInBox.Value) : 0;
                protocoleJournalierArticleComsumed = protocoleJournalierArticleComsumed + protocoleJournalierArticle;
            }

            return protocoleJournalierArticleComsumed;
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

			var serviceProvider = this._scopedServiceProvider.GetScopeServiceProvider();
			var relatedMasterDataServices = serviceProvider.GetServices<IMasterDataRelatedEntity>();

			var hubService = serviceProvider.GetService<IHubContext<UserNotificationHub, IUserNotificationClient>>();
			var masterDataKeys = relatedMasterDataServices.Select(x => x.MasterDataKey).Distinct().ToArray();
			var now = serviceProvider.GetService<ICurrentDateTime>().Now;
			var clients = hubService.Clients.All;
			await clients.SendAsync("MasterDataChanged", new NotificationSignalRModel(Guid.NewGuid(), now, new string[] { "Patients" }));

		}
	}
}