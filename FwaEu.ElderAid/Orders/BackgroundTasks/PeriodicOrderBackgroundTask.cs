using FwaEu.Fwamework.Data.Database.Nhibernate.Tracking;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.DependencyInjection;
using FwaEu.Fwamework.Temporal;
using FwaEu.ElderAid.Orders.Services;
using FwaEu.ElderAid.Organizations;
using FwaEu.Modules.BackgroundTasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FwaEu.ElderAid.Orders.BackgroundTasks
{
    public class PeriodicOrderBackgroundTask : IBackgroundTask
    {
        public const string TaskName = "PeriodicOrder";
        private readonly ILogger<BackgroundTasksBackgroundService> _logger;
        private readonly PeriodicOrderOptions _periodicOrderOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICurrentDateTime _currentDateTime;
        private readonly AsyncLocalScopedServiceProviderAccessor _asyncLocalScopedServiceProviderAccessor;

        private Timer _dailyTimer = null!;
        public PeriodicOrderBackgroundTask(IServiceProvider serviceProvider,
                                     AsyncLocalScopedServiceProviderAccessor asyncLocalScopedServiceProviderAccessor,
                                            ILogger<BackgroundTasksBackgroundService> logger,
                                                IOptions<PeriodicOrderOptions> periodicOrderOptions,
                                                    ICurrentDateTime currentDateTime)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _periodicOrderOptions = periodicOrderOptions?.Value ?? throw new ArgumentNullException(nameof(periodicOrderOptions));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _currentDateTime = currentDateTime ?? throw new ArgumentNullException(nameof(currentDateTime));
            _asyncLocalScopedServiceProviderAccessor = asyncLocalScopedServiceProviderAccessor ?? throw new ArgumentNullException(nameof(asyncLocalScopedServiceProviderAccessor));

        }
        public async Task<string> ExecuteAsync(ITaskStartParameters taskStartParameters, CancellationToken cancellationToken)
        {
            try
            {
                var backgroundTaskRegularityInMinutes = _periodicOrderOptions.BackgroundTaskRegularityInMinutes;
                using (var scope = _serviceProvider.CreateScope())
                using (_asyncLocalScopedServiceProviderAccessor.BeginScope(scope.ServiceProvider))
                {
                    var mainSessionContext = scope.ServiceProvider.GetService<MainSessionContext>();
                    var repositorySession = mainSessionContext.RepositorySession;
                    var organizationRepository = repositorySession.Create<AdminOrganizationEntityRepository>();
                    var organizations = await organizationRepository.QueryNoPerimeter().Where(x => x.IsActive == true).ToListAsync();
                    foreach (var organization in organizations)
                    {
                        double delayToExecuteOrder = GetDelayToExecuteOrder(organization.OrderPeriodicityDays, organization.OrderPeriodicityDayOfWeek, organization.LastPeriodicityOrder);
                        if (delayToExecuteOrder < backgroundTaskRegularityInMinutes)
                        {
                            var scopeFactory = scope.ServiceProvider.GetService<IServiceScopeFactory>();
                            new Timer(async (object state) =>
                            {
                                using (var scope = scopeFactory.CreateScope())
                                using (_asyncLocalScopedServiceProviderAccessor.BeginScope(scope.ServiceProvider))
                                {
                                    var logger = scope.ServiceProvider.GetService<ILogger<PeriodicOrderBackgroundTask>>();
                                    try
                                    {
                                        using (scope.ServiceProvider.GetRequiredService<CreationOrUpdateTrackingEventListener.Disabler>().Disable())
                                        {
                                            var orderService = scope.ServiceProvider.GetService<IOrderService>();
                                            await orderService.CreatePeriodicOrderAsync(organization.Id);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.LogError(ex, "Error during adding background tasks service");
                                    }
                                }
                            }, null, TimeSpan.FromMinutes(delayToExecuteOrder), Timeout.InfiniteTimeSpan);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during background tasks service");
            }
            return string.Empty;
        }

        private double GetDelayToExecuteOrder(int orderPeriodicityDays, int orderPeriodicityDayOfWeek, DateTime lastPeriodicityOrder)
        {
            var options = _periodicOrderOptions;
            var today = _currentDateTime.Now;

            // Calculate the difference between the current date and dateStart
            TimeSpan timeSpanDifference = today - lastPeriodicityOrder;
            // Calculate the number of periods completed
            int periodsCompleted = (int)Math.Ceiling(timeSpanDifference.TotalDays / orderPeriodicityDays);

            var nextPeriodicityOrder = lastPeriodicityOrder.AddDays(periodsCompleted * orderPeriodicityDays);

            nextPeriodicityOrder = nextPeriodicityOrder.Date + new TimeSpan(options.ValidationHour, options.ValidationMinutes, 0);

            int numberOfElements = Enum.GetValues(typeof(DayOfWeek)).Length;

            if (!((int)nextPeriodicityOrder.DayOfWeek == orderPeriodicityDayOfWeek || (int)nextPeriodicityOrder.DayOfWeek + numberOfElements == orderPeriodicityDayOfWeek))
            {
                // Calculate the difference in days to get to the desired dayOfWeek
                int daysToAdd = orderPeriodicityDayOfWeek - (((nextPeriodicityOrder.DayOfWeek == 0) ? numberOfElements : 0)
                                                                   + (int)nextPeriodicityOrder.DayOfWeek);

                nextPeriodicityOrder = nextPeriodicityOrder.AddDays(daysToAdd);
            }

            var totalMinutes = (nextPeriodicityOrder - today).TotalMinutes;
            return totalMinutes >= 0 ? totalMinutes : options.BackgroundTaskRegularityInMinutes;
        }
    }
}