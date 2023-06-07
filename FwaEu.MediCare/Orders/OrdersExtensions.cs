using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.MediCare.Orders
{
    public static class OrdersExtensions
    {
        public static IServiceCollection AddApplicationOrders(this IServiceCollection services, ApplicationInitializationContext context)
        {
            var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
            repositoryRegister.Add<OrderEntityRepository>();

            return services;
        }
    }
}