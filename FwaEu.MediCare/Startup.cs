using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FwaEu.Fwamework.StaticFiles;
using FwaEu.Fwamework.WebApi;
using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.MediCare.Initialization;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Temporal;
using FwaEu.Fwamework.DependencyInjection;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework;
using FwaEu.Fwamework.Setup;
using FwaEu.Fwamework.Imports;
using FwaEu.Modules.Importers.SqlImporter;
using FwaEu.Modules.Importers.ExcelImporter;
using FwaEu.Fwamework.Configuration;
using FwaEu.Modules.GenericImporter;
using FwaEu.Fwamework.ValueConverters;
using Serilog;
using FwaEu.Modules.Logging;
using FwaEu.Modules.Users.CultureSettings;
using FwaEu.Modules.MasterData;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.Users.AvatarsGravatar;
using FwaEu.Modules.Monitoring;
using FwaEu.Fwamework.Formatting;
using FwaEu.Modules.Importers.ExcelImporterGenericImporter;
using FwaEu.Modules.DataImport;
using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.PasswordRecovery;
using Microsoft.Extensions.DependencyInjection.Extensions;
using FwaEu.Modules.Permissions.MasterData;
using FwaEu.MediCare.Application;
using FwaEu.Modules.Users.AdminState;
using FwaEu.Fwamework.Application;
using FwaEu.MediCare.Html;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.NHibernateLogging;
using FwaEu.Modules.Aspose;
using FwaEu.Fwamework.Text;
using FwaEu.Fwamework.Data;
using FwaEu.MediCare.MasterDataPermission;
using Microsoft.AspNetCore.Http;
using FwaEu.Modules.EnumValues;
using FwaEu.Modules.AssemblyProvider;
using FwaEu.MediCare.AssemblyProvider;
using FwaEu.Modules.Authentication.Impersonate;
using FwaEu.Modules.Authentication.JsonWebToken;

using FwaEu.Fwamework.Mail;
using FwaEu.MediCare.Users;
using FwaEu.MediCare.MappingTransformer;
using FwaEu.MediCare.Patients;
using FwaEu.MediCare.Orders;
using FwaEu.MediCare.Referencials;
using FwaEu.MediCare.Organizations;
using FwaEu.MediCare.ViewContext;
using FwaEu.MediCare.GenericSession;
using FwaEu.Modules.Permissions.ByRole;

namespace FwaEu.MediCare
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment)
        {

            var builder = new ConfigurationBuilder()
                 .SetBasePath(environment.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

            //NOTE: This code allow us to have composed environments names
            //// ex: for "Production.VilliersLeBel" environment name, it will try to load appSettings.Production.json and appSettings.VilliersLebBel.json
            foreach (var environmentNamePart in environment.EnvironmentName.Split("."))
            {
                builder = builder.AddJsonFile($"appsettings.{environmentNamePart}.json", optional: true, reloadOnChange: true);
                builder = builder.AddJsonFile($"appsettings.{environmentNamePart}.Local.json", optional: true, reloadOnChange: true);
            }

            builder = builder.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.Local.json", optional: true, reloadOnChange: true)
            
            .AddEnvironmentVariables();

            this._configuration = builder.Build();
            this._environment = environment;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(this._configuration)
                .CreateLogger();
        }

        private readonly IConfigurationRoot _configuration;
        private readonly IWebHostEnvironment _environment;

        public void ConfigureServices(IServiceCollection services)
        {
            using (var context = new ApplicationInitializationContext(this._configuration, this._environment))
            {
                context.ServiceStore.Add<IRepositoryRegister>(new RepositoryRegister());

                services.RemoveAll<IConfiguration>();
                services.RemoveAll<IConfigurationRoot>();
                services.AddSingleton<IConfiguration>(this._configuration);
                services.AddSingleton<IConfigurationRoot>(this._configuration);

                services.AddCors();
                services.AddMemoryCache();
                services.AddResponseCompression();
                services.AddHttpContextAccessor();

                services.AddFwameworkDependencyInjection();
                services.AddFwameworkStaticFilesServices();
                services.AddFwameworkTemporal();
                services.AddFwameworkTextServices();
                services.AddFwameworkMvc(context);
                services.AddJsonWebTokenServices(context);
                var authenticationFeatures = services.AddFwameworkAuthentication(context);
                services.AddImpersonateServices(context, authenticationFeatures);
                services.AddFwameworkWebApiAllower(context);
                services.AddFwameworkSessionsServices();
                services.AddFwameworkNhibernate();
                services.AddFwameworkDatabase();
                services.AddFwameworkPermissions(context);
                services.AddFwameworkSetup(context);
                services.AddFwameworkImportServices();
                services.AddFwameworkValueConverters();
                services.AddFwameworkConfigurations();
                services.AddFwameworkUsersServices();
                services.AddFwameworkApplicationAllower(context);
                services.AddFwameworkGlobalization();
                services.AddFwameworkDataFileServices();
                services.AddFwameworkFormatting();
                services.AddFwameworkModelValidation();

                services.AddFwameworkModuleDataImport();
                services.AddFwameworkModuleGenericImporter();
                services.AddFwameworkModuleSqlImportServices();
                services.AddFwameworkModuleExcelImportServices();
                services.AddFwameworkModuleExcelGenericImporter();
                services.AddFwameworkModuleCultureSettings(context);
                services.AddFwameworkModuleSerilog(context);
                services.AddFwameworkModuleAdminState(context);
                services.AddFwameworkModuleMasterDataServices();
                services.AddFwameworkModuleGravatar(context);
                services.AddFwameworkModuleMonitoring();
                services.AddFwameworkModuleAllowApplicationEnabler(context);
                services.AddFwameworkModuleGenericAdmin();
                services.AddFwameworkModuleUserPasswordRecovery(context);
                services.AddFwameworkModulePermissionsMasterData();
				services.AddFwameworkModuleEnumValues();
				services.AddFwameworkModuleAssemblyProvider();


				services.AddFwameworkModuleNHibernateLogging(context);
                services.AddFwameworkModuleAspose(typeof(Aspose.Cells.License)); //NOTE: You can add other Aspose licenses depending on your application needs

                services.AddFwameworkModulePermissionsByRole(context);
                services.AddMasterDataPermissions();

                services.AddApplicationInformation(context);
                services.AddRelativePathProviders(this._environment);
                services.AddApplicationDatabase(context, authenticationFeatures);
                services.AddApplicationUserServices(context);
                services.AddApplicationMailServices(context);
				services.AddMailListenersServices();
				services.AddApplicationGlobalization();
                services.AddApplicationSwagger(authenticationFeatures, context);
                services.AddFwaEuMediCareAssemblyProviderExtension();

                services.AddApplicationOrganizations(context);
                services.AddApplicationGenericSession();
                services.AddApplicationMappingTransformer();
                services.AddApplicationReferencials(context);
                services.AddApplicationPatients(context);
                services.AddApplicationOrders(context);

                services.AddApplicationHtml();

                services.AddApplicationViewContext();
            }
        }

        public void Configure(IApplicationBuilder application, IApplicationInfo applicationInfo,
            ILoggerFactory loggerFactory, INHibernateLoggingInfo nHibernateLoggingInfo)
        {
            bool IsNotSetupRoute(HttpContext context)
            {
                return !context.Request.Path.StartsWithSegments("/Setup");
            }

            ApplicationServices.Initialize(application.ApplicationServices);

            application.ConfigureFwameworkSerilog(this._configuration, this._environment, applicationInfo);
            application.ConfigureNHibernateLogging(loggerFactory, nHibernateLoggingInfo);

            application.UseResponseCompression();

            if (this._environment.IsLikeDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }
            else if (this._configuration.GetSection("Application:UseHttpsRedirection").Get<bool>())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                application.UseHsts();
                application.UseHttpsRedirection();
            }

            application.UseApplicationSwagger(this._configuration);
            application.UseFwameworkModuleLog();

            application.UseCors(options => options
                .SetIsOriginAllowed(_ => true) // NOTE: AllowAnyOrigin() not usable with AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            application.UseAuthentication();
            application.UseRouting();
            application.UseAuthorization();

            application.UseWhen(IsNotSetupRoute, application => application
                    .UseFwameworkCurrentUser()
            );

            application.UseApplicationViewContext();

            application.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
