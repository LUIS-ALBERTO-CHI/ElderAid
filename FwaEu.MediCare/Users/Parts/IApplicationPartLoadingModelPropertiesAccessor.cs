using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Users
{
	public interface IApplicationPartLoadingListModelPropertiesAccessor
	{
		string FirstName { get; }
		string LastName { get; }
		string Email { get; }
		string Login { get; }
	}

	public interface IApplicationPartLoadingModelPropertiesAccessor
		: IApplicationPartLoadingListModelPropertiesAccessor
	{
	}
}
