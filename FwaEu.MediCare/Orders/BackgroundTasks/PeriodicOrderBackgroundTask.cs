using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Temporal;
using FwaEu.MediCare.Orders.Services;
using FwaEu.MediCare.Organizations;
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

namespace FwaEu.MediCare.Orders.BackgroundTasks
{
    public class PeriodicOrderBackgroundTask : IBackgroundTask
    {
        public const string TaskName = "PeriodicOrder";
        private readonly ILogger<BackgroundTasksBackgroundService> _logger;
        private readonly PeriodicOrderOptions _periodicOrderOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICurrentDateTime _currentDateTime;

        private Timer _dailyTimer = null!;
        public PeriodicOrderBackgroundTask(IServiceProvider serviceProvider, 
                                            ILogger<BackgroundTasksBackgroundService> logger, 
                                                IOptions<PeriodicOrderOptions> periodicOrderOptions,
                                                    ICurrentDateTime currentDateTime)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _periodicOrderOptions = periodicOrderOptions?.Value ?? throw new ArgumentNullException(nameof(periodicOrderOptions));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _currentDateTime = currentDateTime ?? throw new ArgumentNullException(nameof(currentDateTime));
        }
        public async Task<string> ExecuteAsync(ITaskStartParameters taskStartParameters, CancellationToken cancellationToken)
        {
            try
            {
                var backgroundTaskRegularityInMinutes = _periodicOrderOptions.BackgroundTaskRegularityInMinutes;
                var scope = _serviceProvider.CreateScope();
                
                using (scope)
                {
                    var mainSessionContext = scope.ServiceProvider.GetService<MainSessionContext>();
                    var organizationRepository = mainSessionContext.RepositorySession.Create<AdminOrganizationEntityRepository>();
                    var organizations = await organizationRepository.QueryNoPerimeter().Where(x => x.IsActive == true).ToListAsync();
                    foreach (var organization in organizations)
                    {
                        double delayToExecuteOrder = GetDelayToExecuteOrder(organization.OrderPeriodicityDays, organization.OrderPeriodicityDayOfWeek, organization.LastPeriodicityOrder);
                        if (delayToExecuteOrder < backgroundTaskRegularityInMinutes)
                            _dailyTimer = GetTimerToExecuteOrder(delayToExecuteOrder, organization.Id, organization.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during background tasks service");
            }
            return string.Empty;
        }

        private Timer GetTimerToExecuteOrder(double delayToSendMail, int organizationId, string organizationName)
        {
            var scope = _serviceProvider.CreateScope();
            return new Timer(async (object state) =>
            {
                using (scope)
                {
                    var logger = scope.ServiceProvider.GetService<ILogger<PeriodicOrderBackgroundTask>>();
                    try
                    {
                        logger.LogInformation("Timer begin scheddle for {0} on: {1}", organizationName, _currentDateTime.Now);
                        var orderService = scope.ServiceProvider.GetService<IOrderService>();
                        await orderService.CreatePeriodicOrderAsync(organizationId);
                        logger.LogInformation("Timer end scheddle for {0} on: {1}", organizationName, _currentDateTime.Now);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error during adding background tasks service");
                    }
                }
            }, null, TimeSpan.FromMinutes(delayToSendMail), Timeout.InfiniteTimeSpan);
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