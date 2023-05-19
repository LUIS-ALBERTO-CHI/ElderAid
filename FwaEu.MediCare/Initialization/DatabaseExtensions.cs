using FwaEu.Fwamework;
using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Users;
using FwaEu.MediCare.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FwameworkDataBaseExtensions = FwaEu.Fwamework.Data.Database.DatabaseExtensions;

namespace FwaEu.MediCare.Initialization
{
    public static class DatabaseExtensions
    {
        private static IEnumerable<Type> GetTypesInWhichSearchForMappings(IAuthenticationFeatures authenticationFeatures)
        {
            return authenticationFeatures.CustomizeTypesInWhichSearchForMappings(
                typeof(FwameworkDataBaseExtensions).Assembly.GetExportedTypes()
                .Concat(typeof(FwaEu.Modules.Data.Database.MsSql.MsSqlDatabaseFeatures).Assembly.GetExportedTypes()
                .Concat(typeof(DatabaseExtensions).Assembly.GetExportedTypes())));
        }

        private static IEnumerable<INhibernateConfigurationLoader> GetLoaders(
            IServiceProvider serviceProvider,
            IAuthenticationFeatures authenticationFeatures, 
            NhibernateConfigurationOptions configurationOptions)
        {
            yield return new OptionsBasedNhibernateConfigurationLoader(
                serviceProvider: serviceProvider,
                options: configurationOptions,
                typesInWhichSearchForMappings: GetTypesInWhichSearchForMappings(authenticationFeatures));

        }

        private static void AddDatabaseFeatures(this IServiceCollection services, NhibernateConfigurationOptions configurationOptions)
        {
            services.AddTransient<IDatabaseFeaturesProvider>(sp => 
                    new DatabaseFeaturesProvider(configurationOptions.DatabaseFeaturesTypeFullName));
        }

        public static IServiceCollection AddApplicationDatabase(
            this IServiceCollection services,
            ApplicationInitializationContext context,
            IAuthenticationFeatures authenticationFeatures)
        {
            var repositories = context.ServiceStore.Get<IRepositoryRegister>();
            repositories.Add<UserEntity, ApplicationUserEntity, IUserEntityRepository, ApplicationUserEntityRepository>();

            var section = context.Configuration.GetRequiredSection("Application:Nhibernate");
            var configurationOptions = section.Get<NhibernateConfigurationOptions>();

            services.AddSingleton(repositories);
            services.AddDatabaseFeatures(configurationOptions);
            services.AddTransient((serviceProvider) => GetLoaders(serviceProvider, authenticationFeatures, configurationOptions));

            return services;
        }
    }
}