using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.Permissions.ByIsAdmin
{
	public class ByIsAdminPermissionManager : IPermissionManager
	{
		public Task<bool> HasPermissionAsync(UserEntity user, IPermission permission)
		{
			return Task.FromResult(user.IsAdmin);
		}
	}
}
