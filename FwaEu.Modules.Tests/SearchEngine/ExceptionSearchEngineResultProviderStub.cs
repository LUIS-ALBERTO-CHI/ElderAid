using FwaEu.Modules.SearchEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.SearchEngine
{
	public class ExceptionSearchEngineResultProviderStub : ISearchEngineResultProvider
	{
		public Task<IEnumerable<object>> SearchAsync(string search, SearchPagination pagination)
		{
			return Task.FromException<IEnumerable<object>>(new ApplicationException("Dummy"));
		}
	}
}
