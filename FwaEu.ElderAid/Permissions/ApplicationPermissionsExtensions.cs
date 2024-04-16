using FwaEu.ElderAid.Permissions.ByIsAdmin;
using FwaEu.Fwamework.Permissions;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.ElderAid.Permissions
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
