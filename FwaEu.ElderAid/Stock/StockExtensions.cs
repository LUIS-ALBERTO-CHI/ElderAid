using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using FwaEu.Fwamework;
using FwaEu.ElderAid.Stock.Services;
using FwaEu.ElderAid.Stock.MasterData;

namespace FwaEu.ElderAid.Stock
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