using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.MediCare.Orders
{
    public static class OrdersExtensions
    {
        public static IServiceCollection AddApplicationOrders(this IServiceCollection services)
        {
            return services;
        }
    }
}