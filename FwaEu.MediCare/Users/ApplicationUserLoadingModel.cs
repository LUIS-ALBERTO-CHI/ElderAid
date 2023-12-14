using FwaEu.Fwamework.Users;
using FwaEu.Modules.Users.AdminState.Parts;
using FwaEu.Modules.Users.HistoryPart.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Users
{
	public class ApplicationUserLoadingModel : FwaEu.Fwamework.Users.UserEditModel
		, IHistoryPartLoadingModelPropertiesAccessor
		, IAdminStatePartLoadingModelPropertiesAccessor
		, IApplicationPartLoadingModelPropertiesAccessor
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Login { get; set; }


		public int? CreatedById { get; set; }
		public DateTime CreatedOn { get; set; }
		public int? UpdatedById { get; set; }
		public DateTime UpdatedOn { get; set; }
		public bool IsAdmin { get; set; }
		public UserState State { get; set; }
	}
}
