﻿using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using FwaEu.MediCare.Orders.Services;
using FwaEu.MediCare.Orders.MasterData;
using FwaEu.Fwamework;
using FwaEu.Modules.BackgroundTasks;
using System;
using FwaEu.MediCare.Orders.BackgroundTasks;
using System.Linq;
using FwaEu.Fwamework.Data.Database.Nhibernate;

namespace FwaEu.MediCare.Orders
{
    public static class OrdersExtensions
    {
        public static IServiceCollection AddApplicationOrders(this IServiceCollection services, ApplicationInitializationContext context)
        {
            var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
            repositoryRegister.Add<OrderEntityRepository>();
            repositoryRegister.Add<PeriodicOrderValidationEntityRepository>();

            services.AddTransient<IOrderService, OrderService>();

            services.AddMasterDataProvider<OrderMasterDataProvider>("Orders");
            services.AddMasterDataProvider<PeriodicOrderValidationMasterDataProvider>("PeriodicOrderValidations");

            var periodicOrderSection = context.Configuration.GetSection("Application:PeriodicOrder");
            services.Configure<PeriodicOrderOptions>(periodicOrderSection);

            var backgroundTaskRegularityInMinutesSection = periodicOrderSection.GetChildren().FirstOrDefault(e => e.Key == "BackgroundTaskRegularityInMinutes");
            var backgroundTaskRegularityInMinutes = Convert.ToDouble(backgroundTaskRegularityInMinutesSection.Value);
            services.AddScheduledBackgroundTask<PeriodicOrderBackgroundTask>
                            (PeriodicOrderBackgroundTask.TaskName, TimeSpan.FromMinutes(backgroundTaskRegularityInMinutes));

            services.AddTransient<IEventListenersInitializer, UpdatingOrganizationEventListenerInitializer>();

            return services;
        }
    }
}