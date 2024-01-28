using Microsoft.Extensions.DependencyInjection;
using FwaEu.Modules.MasterData;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework;
using FwaEu.ElderAid.Treatments.Services;
using FwaEu.ElderAid.Treatments.MasterData;

namespace FwaEu.ElderAid.Treatments
{
    public static class TreatmentsExtension
    {
        public static IServiceCollection AddApplicationTreatments(this IServiceCollection services, ApplicationInitializationContext context)
        {
            var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();

            repositoryRegister.Add<TreatmentEntityRepository>();
            services.AddTransient<ITreatmentService, TreatmentService>();

            services.AddMasterDataProvider<TreatmentMasterDataProvider>("Treatments");
            return services;
        }
    }
}