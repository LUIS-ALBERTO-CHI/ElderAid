using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using FwaEu.MediCare.Patients.MasterData;
using FwaEu.Fwamework;
using FwaEu.MediCare.Patients.Services;
using FwaEu.Modules.GenericAdmin;
using FwaEu.MediCare.Patients.GenericAdmin;

namespace FwaEu.MediCare.Patients
{
    public static class PatientsExtensions
    {
        public static IServiceCollection AddApplicationPatients(this IServiceCollection services, ApplicationInitializationContext context)
        {
            var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
            repositoryRegister.Add<PatientEntityRepository>();
            repositoryRegister.Add<IncontinenceLevelEntityRepository>();

            services.AddTransient<IPatientService, PatientService>();

            services.AddMasterDataProvider<IncontinenceLevelMasterDataProvider>("IncontinenceLevels");
            services.AddMasterDataProvider<PatientMasterDataProvider>("Patients");

            services.AddTransient<IGenericAdminModelConfiguration, IncontinenceLevelEntityToModelGenericAdminModelConfiguration>();

            return services;
        }
    }
}