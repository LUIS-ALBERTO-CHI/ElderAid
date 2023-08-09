using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using FwaEu.Fwamework;
using FwaEu.MediCare.Stock.Services;
using FwaEu.MediCare.Stock.MasterData;

namespace FwaEu.MediCare.Stock
{
    public static class StockExtensions
    {
        public static IServiceCollection AddApplicationStock(this IServiceCollection services, ApplicationInitializationContext context)
        {
            var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();

            services.AddTransient<IStockService, StockService>();
            repositoryRegister.Add<StockConsumptionEntityRepository>();

            services.AddMasterDataProvider<StockConsumptionMasterDataProvider>("StockConsumptions");
            return services;
        }
    }
}