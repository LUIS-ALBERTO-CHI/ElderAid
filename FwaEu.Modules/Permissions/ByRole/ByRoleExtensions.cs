using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.MasterData;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using FwaEu.Fwamework.Users.WebApi;
using FwaEu.Modules.Permissions.ByRole.MasterData;
using FwaEu.Modules.Permissions.ByRole.Part;
using FwaEu.Modules.Permissions.ByRole.Part.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.Permissions.ByRole.GenericAdmin;
using FwaEu.Modules.SimpleMasterData;
using FwaEu.Modules.SimpleMasterData.MasterData;
using FwaEu.Modules.SimpleMasterData.GenericAdmin;

namespace FwaEu.Modules.Permissions.ByRole
{
	public static class ByRoleExtensions
	{
		public static IServiceCollection AddFwameworkModulePermissionsByRole(this IServiceCollection services, ApplicationInitializationContext context)
		{
			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<RoleEntityRepository>();
			repositoryRegister.Add<RolePermissionEntityRepository>();
			repositoryRegister.Add<UserRoleEntityRepository>();

			services.AddTransient<IPermissionManager, ByRolePermissionManager>();

			services.AddTransient<IPartHandler, RolePartHandler>();

			services.For<RoleEntity>(context)
			 .AddRepository<RoleEntityRepository>()
			 .AddMasterDataProviderFactory()	
			 .AddGenericAdminModelConfiguration();

			services.AddMasterDataProvider<RolePermissionMasterDataProvider>("RolePermissions");

			services.AddTransient<IGenericAdminModelConfiguration, RoleEntityToModelGenericAdminModelConfiguration>();
			services.AddTransient<IGenericAdminModelConfiguration, RolePermissionEntityToModelGenericAdminModelConfiguration>();
            services.AddTransient<IPermissionProviderFactory, DefaultPermissionProviderFactory<UserRolePermissionProvider>>();

            return services;
		}
	}
}
