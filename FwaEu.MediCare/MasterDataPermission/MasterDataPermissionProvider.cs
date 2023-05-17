using FwaEu.Fwamework.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.MasterDataPermission
{
	public class MasterDataPermissionProvider : ReflectedPermissionProvider
	{
		public IPermission CanAdminstrateApplicationMasterData { get; set; }
	}
	
}
