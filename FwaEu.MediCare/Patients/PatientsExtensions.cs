﻿using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using FwaEu.MediCare.Patients.MasterData;
using FwaEu.Fwamework;
using FwaEu.MediCare.Patients.Services;
using FwaEu.Fwamework.Permissions;
using FwaEu.TemplateCore.FarmManager;

namespace FwaEu.MediCare.Patients
{
    public static class PatientsExtensions
    {
        public static IServiceCollection AddApplicationPatients(this IServiceCollection services, ApplicationInitializationContext context)
        {
            var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
            repositoryRegister.Add<PatientEntityRepository>();

            services.AddTransient<IPermissionProviderFactory, DefaultPermissionProviderFactory<PatientPermissionProvider>>();

            services.AddTransient<IPatientService, PatientService>();

            services.AddMasterDataProvider<PatientMasterDataProvider>("Patients");

            return services;
        }
    }
}