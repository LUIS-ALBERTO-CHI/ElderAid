using FwaEu.Fwamework.Permissions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.MasterDataPermission
{
	public static class MasterDataPermissionsExtensions
	{
		public static IServiceCollection AddMasterDataPermissions(this IServiceCollection services)
		{
			services.AddTransient<IPermissionProviderFactory, DefaultPermissionProviderFactory<MasterDataPermissionProvider>>();

			return services;
		}
	}
}
