using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public class UserUpdateNotAllowedException : ApplicationException
	{
		public UserUpdateNotAllowedException(string message) : base(message)
		{
		}
	}
}
