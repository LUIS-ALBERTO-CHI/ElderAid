using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;

using FwaEu.Fwamework;
using FwaEu.MediCare.Organizations.MasterData;
using FwaEu.Modules.GenericAdmin;

using FwaEu.MediCare.Organizations.GenericAdmin;
using FwaEu.Fwamework.Users.Parts;
using FwaEu.MediCare.Organizations.Part.Services;

namespace FwaEu.MediCare.Organizations
{
    public static class OrganizationsExtensions
    {
        public static IServiceCollection AddApplicationOrganizations(this IServiceCollection services, ApplicationInitializationContext context)
        {
            var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
            repositoryRegister.Add<OrganizationEntityRepository>();
            repositoryRegister.Add<AdminOrganizationEntityRepository>();
            repositoryRegister.Add<OrganizationUserLinkEntityRepository>();

            services.AddMasterDataProvider<OrganizationEntityMasterDataProvider>("Organizations");
            services.AddMasterDataProvider<AdminOrganizationEntityMasterDataProvider>("AdminOrganizations");

            services.AddMasterDataProvider<UserOrganizationsMasterDataProvider>("UserOrganizations");

            services.AddTransient<IGenericAdminModelConfiguration, OrganizationEntityToModelGenericAdminModelConfiguration>();

            services.AddTransient<IPartHandler, OrganizationPartHandler>();

            return services;
        }
    }
}