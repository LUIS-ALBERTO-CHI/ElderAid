using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.EnumValues
{
	public class EnumNotFoundException : ApplicationException
	{
		public EnumNotFoundException(string message) : base(message)
		{
		}
	}
}
