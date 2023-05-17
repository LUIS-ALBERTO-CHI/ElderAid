using FwaEu.MediCare.Permissions.ByIsAdmin;
using FwaEu.Fwamework.Permissions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Permissions
{
	public static class ApplicationPermissionsExtensions
	{
		public static IServiceCollection AddApplicationPermissionsByIsAdmin(this IServiceCollection services)
		{
			services.AddTransient<IPermissionManager, ByIsAdminPermissionManager>();

			return services;
		}
	}
}
