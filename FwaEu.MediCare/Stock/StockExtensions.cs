using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using FwaEu.Fwamework;
using FwaEu.MediCare.Stock.Services;

namespace FwaEu.MediCare.Stock
{
    public static class StockExtensions
    {
        public static IServiceCollection AddApplicationStock(this IServiceCollection services, ApplicationInitializationContext context)
        {
            var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();

            services.AddTransient<IStockService, StockService>();

            return services;
        }
    }
}