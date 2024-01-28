using FwaEu.Fwamework.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.FarmManager
{
	public class PatientPermissionProvider : ReflectedPermissionProvider
	{
		public IPermission CanChangeIncontinenceLevel { get; set; }
	}
}
