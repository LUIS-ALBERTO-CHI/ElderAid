using Microsoft.Extensions.DependencyInjection;
using FwaEu.Modules.MasterData;
using FwaEu.MediCare.Referencials.MasterData;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework;
using FwaEu.Modules.GenericAdmin;
using FwaEu.MediCare.Referencials.GenericAdmin;

namespace FwaEu.MediCare.Referencials
{
    public static class ReferencialsExtensions
    {
        public static IServiceCollection AddApplicationReferencials(this IServiceCollection services, ApplicationInitializationContext context)
        {
            var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
            repositoryRegister.Add<BuildingEntityRepository>();

            repositoryRegister.Add<DosageFormEntityRepository>();

            repositoryRegister.Add<CabinetEntityRepository>();
            repositoryRegister.Add<ProtectionDosageEntityRepository>();
            repositoryRegister.Add<IncontinenceLevelEntityRepository>();

            services.AddMasterDataProvider<BuildingMasterDataProvider>("Buildings");
            services.AddMasterDataProvider<DosageFormMasterDataProvider>("DosageForms");
            services.AddMasterDataProvider<CabinetMasterDataProvider>("Cabinets");
            services.AddMasterDataProvider<ProtectionDosageMasterDataProvider>("ProtectionDosages");
            services.AddMasterDataProvider<IncontinenceLevelMasterDataProvider>("IncontinenceLevels");

            services.AddTransient<IGenericAdminModelConfiguration, DosageFormEntityToModelGenericAdminModelConfiguration>();
            services.AddTransient<IGenericAdminModelConfiguration, IncontinenceLevelEntityToModelGenericAdminModelConfiguration>();

            return services;
        }
    }
}