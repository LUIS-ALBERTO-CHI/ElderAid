using FwaEu.Fwamework.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FwaEu.MediCare.Users.UserGroups
{
	public class UserGroupPermissionProvider : ReflectedPermissionProvider
	{
		public IPermission CanAdministrateUserGroups { get; set; }
	}
}
