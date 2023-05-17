using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FwaEu.Modules.GenericAdmin
{
	public class AuthorizationException : ApplicationException
	{
		public AuthorizationException(string actionName)
			: base($"Action '{actionName}' not authorized.")
		{
		}
	}
}