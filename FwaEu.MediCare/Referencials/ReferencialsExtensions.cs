using Microsoft.Extensions.DependencyInjection;
using FwaEu.MediCare.Referencials.Services;
using FwaEu.Modules.MasterData;
using FwaEu.MediCare.Referencials.MasterData;

namespace FwaEu.MediCare.Referencials
{
    public static class ReferencialsExtensions
    {
        public static IServiceCollection AddApplicationReferencials(this IServiceCollection services)
        {
            services.AddTransient<IBuildingService, BuildingService>();

            services.AddMasterDataProvider<BuildingMasterDataProvider>("Buildings");

            //services.AddTransient<BuildingMasterDataProvider>();
            return services;
        }
    }
}