using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.WebApi
{
	public class AllowedApplicationsOptions
	{
		public ApplicationEntry[] Applications { get; set; }
	}

	public class ApplicationEntry
	{
		public string Name { get; set; }
		public string Secret { get; set; }
		public Filter Filter { get; set; }
	}

	public class Filter
	{
		public string[] Allowed { get; set; }
	}
}
