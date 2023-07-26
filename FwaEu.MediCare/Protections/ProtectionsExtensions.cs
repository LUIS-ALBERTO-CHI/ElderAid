using Microsoft.Extensions.DependencyInjection;
using FwaEu.Modules.MasterData;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework;
using FwaEu.MediCare.Protections.MasterData;
using FwaEu.MediCare.Protections.Services;

namespace FwaEu.MediCare.Protections
{
    public static class ProtectionsExtensions
    {
        public static IServiceCollection AddApplicationProtections(this IServiceCollection services, ApplicationInitializationContext context)
        {
            var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
            repositoryRegister.Add<ProtectionEntityRepository>();
            services.AddMasterDataProvider<ProtectionMasterDataProvider>("Protections");

            services.AddTransient<IProtectionService, ProtectionService>();
            return services;
        }
    }
}