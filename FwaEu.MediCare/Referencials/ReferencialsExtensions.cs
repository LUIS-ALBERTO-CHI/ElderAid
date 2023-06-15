using Microsoft.Extensions.DependencyInjection;
using FwaEu.MediCare.Referencials.Services;
using FwaEu.Modules.MasterData;
using FwaEu.MediCare.Referencials.MasterData;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework;

namespace FwaEu.MediCare.Referencials
{
    public static class ReferencialsExtensions
    {
        public static IServiceCollection AddApplicationReferencials(this IServiceCollection services, ApplicationInitializationContext context)
        {
            var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
            repositoryRegister.Add<BuildingEntityRepository>();
            services.AddTransient<IBuildingService, BuildingService>();

            services.AddMasterDataProvider<BuildingMasterDataProvider>("Buildings");

            return services;
        }
    }
}