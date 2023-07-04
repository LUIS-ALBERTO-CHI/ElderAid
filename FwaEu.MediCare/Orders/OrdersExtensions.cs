using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using FwaEu.MediCare.Orders.Services;
using FwaEu.MediCare.Orders.MasterData;
using FwaEu.Fwamework;

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

            return services;
        }
    }
}