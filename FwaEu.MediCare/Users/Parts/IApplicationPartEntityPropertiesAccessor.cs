using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Users
{
	interface IApplicationPartEntityPropertiesAccessor
	{
		int Id { get; }
		string FirstName { set; }
		string LastName { set; }
		string Email { get; set; }
		public string Login { get; set; }
	}
}
