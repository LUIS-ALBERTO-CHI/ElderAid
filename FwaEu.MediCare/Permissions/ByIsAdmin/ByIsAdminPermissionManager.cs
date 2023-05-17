using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Permissions.ByIsAdmin
{
	public class ByIsAdminPermissionManager  : IPermissionManager
	{
		public Task<bool> HasPermissionAsync(UserEntity user, IPermission permission)
		{
			return Task.FromResult(user.IsAdmin);
		}
	}
}
