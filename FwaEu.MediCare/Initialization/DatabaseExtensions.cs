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
            var sectionConnectionStrings = serviceProvider.GetService<IConfiguration>().GetSection("ConnectionStrings");
            var settingsConnectionStrings = sectionConnectionStrings.Get<Dictionary<string, string>>();

            //NOTE: Get all types from all projects (Fwamwork + Modules + Current assembly)
            var typesFromAllAssemblies = GetTypesInWhichSearchForMappings(authenticationFeatures);

            foreach (var connectionString in settingsConnectionStrings)
            {
                var keyofConnectionString = connectionString.Key;
                var specificTypesForMappings = typesFromAllAssemblies
                                                .Where(x => (!x.IsDefined(typeof(ConnectionStringAttribute))
                                                                && keyofConnectionString == "Default")
                                                            || (x.IsDefined(typeof(ConnectionStringAttribute))
                                                                && ((ConnectionStringAttribute)Attribute.GetCustomAttribute(x,
                                                                    typeof(ConnectionStringAttribute)))?.Name == keyofConnectionString));
                yield return new CodeBasedNhibernateConfigurationLoader(
                    serviceProvider: serviceProvider,
                    connectionStringName: keyofConnectionString,
                    databaseFeaturesType: typeof(FwaEu.Modules.Data.Database.MsSql.MsSqlDatabaseFeatures),
                    typesInWhichSearchForMappings: specificTypesForMappings,
                    configureDatabaseProperties: (properties) =>
                    {
                        properties.Dialect<MsSql2012Dialect>();
                        properties.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                    },
                    finalizeConfiguration: (configuration) =>
                    {
                        configuration.Properties[NHibernate.Cfg.Environment.DefaultFlushMode] = NHibernate.FlushMode.Manual.ToString();
                    }
                );
            }

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
