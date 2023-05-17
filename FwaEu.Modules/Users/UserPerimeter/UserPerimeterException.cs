using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.UserPerimeter
{
	public class UserPerimeterException : ApplicationException
	{
		public UserPerimeterException(string message) : base(message)
		{
		}
	}
}
