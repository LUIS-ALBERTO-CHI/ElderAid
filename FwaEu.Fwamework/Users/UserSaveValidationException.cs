using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public class UserSaveValidationException : ApplicationException
	{
		public UserSaveValidationException(string userPart, string errorType, string message) : base(message)
		{
			this.ErrorType = errorType
				?? throw new ArgumentNullException(nameof(errorType));
			this.UserPart = userPart;	
		}

		public string ErrorType { get; }
		public string UserPart { get; }
	}
}
