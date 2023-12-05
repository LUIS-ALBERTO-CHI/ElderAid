using FwaEu.Fwamework.Users;
using FwaEu.Modules.Users.HistoryPart.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Users
{
	public class ApplicationUserLoadingListModel : FwaEu.Fwamework.Users.UserListModel
		, IUpdateHistoryPartLoadingModelPropertiesAccessor
		, IApplicationPartLoadingListModelPropertiesAccessor
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public UserState State { get; set; }
		public int? UpdatedById { get; set; }
		public DateTime UpdatedOn { get; set; }
	}
}