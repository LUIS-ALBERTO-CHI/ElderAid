using Microsoft.Extensions.DependencyInjection;
using FwaEu.Modules.MasterData;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework;
using FwaEu.MediCare.Treatments.Services;
using FwaEu.MediCare.Treatments.MasterData;

namespace FwaEu.MediCare.Treatments
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