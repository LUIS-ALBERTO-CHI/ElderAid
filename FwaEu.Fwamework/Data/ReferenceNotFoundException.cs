using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data
{
	public class ReferenceNotFoundException : ApplicationException
	{
		public ReferenceNotFoundException(string message)
			: base(message)
		{
		}

		public ReferenceNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
