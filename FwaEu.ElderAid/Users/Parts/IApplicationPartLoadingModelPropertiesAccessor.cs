using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.Users
{
	public interface IApplicationPartLoadingListModelPropertiesAccessor
	{
		string FirstName { get; }
		string LastName { get; }
		string Email { get; }
		public UserState State { get; set; }
		string Login { get; }
	}

	public interface IApplicationPartLoadingModelPropertiesAccessor
		: IApplicationPartLoadingListModelPropertiesAccessor
	{
	}
}
