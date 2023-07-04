using Microsoft.Extensions.DependencyInjection;
using FwaEu.Modules.MasterData;
using FwaEu.MediCare.Referencials.MasterData;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework;
using FwaEu.MediCare.Referencials.Services;
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

            repositoryRegister.Add<ArticleEntityRepository>();
            services.AddTransient<IArticleService, ArticleService>();

            repositoryRegister.Add<TreatmentEntityRepository>();
            services.AddTransient<ITreatmentService, TreatmentService>();

            repositoryRegister.Add<DosageFormEntityRepository>();

            repositoryRegister.Add<CabinetEntityRepository>();
            repositoryRegister.Add<ProtectionDosageEntityRepository>();
            repositoryRegister.Add<ProtectionEntityRepository>();

            services.AddMasterDataProvider<BuildingMasterDataProvider>("Buildings");
            services.AddMasterDataProvider<ArticleMasterDataProvider>("Articles");
            services.AddMasterDataProvider<TreatmentMasterDataProvider>("Treatments");
            services.AddMasterDataProvider<DosageFormMasterDataProvider>("DosageForms");
            services.AddMasterDataProvider<CabinetMasterDataProvider>("Cabinets");
            services.AddMasterDataProvider<ProtectionDosageMasterDataProvider>("ProtectionDosages");
            services.AddMasterDataProvider<ProtectionMasterDataProvider>("Protections");

            services.AddTransient<IGenericAdminModelConfiguration, DosageFormEntityToModelGenericAdminModelConfiguration>();

            return services;
        }
    }
}