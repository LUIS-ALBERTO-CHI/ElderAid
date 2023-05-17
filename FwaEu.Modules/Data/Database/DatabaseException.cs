using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Data.Database
{
	public class DatabaseException : ApplicationException
	{
		public string Type
		{
			get { return (string)this.Data["Type"]; }
			private set { this.Data["Type"] = value; }
		}

		public DatabaseException(string message, string type, Exception innerException)
			: base(message, innerException)
		{
			this.Type = type ?? throw new ArgumentNullException(nameof(type));
		}
	}
}
