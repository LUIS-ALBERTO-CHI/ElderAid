using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users.WebApi
{
	public class UserSaveValidationErrorModel
	{
		public UserSaveValidationErrorModel(string message, string errorType)
		{
			this.Message = message
				?? throw new ArgumentNullException(nameof(message));
			
			this.ErrorType = errorType
				?? throw new ArgumentNullException(nameof(errorType));
		}

		public string Message { get; }
		public string ErrorType { get; }
	}
}
