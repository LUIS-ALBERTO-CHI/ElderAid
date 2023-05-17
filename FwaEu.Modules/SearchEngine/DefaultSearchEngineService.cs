using FwaEu.Fwamework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.SearchEngine
{
	public class DefaultSearchEngineService : ISearchEngineService
	{
		private readonly IEnumerable<ISearchEngineResultProviderFactory> _factories;

		public DefaultSearchEngineService(IEnumerable<ISearchEngineResultProviderFactory> factories)
		{
			this._factories = factories
				?? throw new ArgumentNullException(nameof(factories));
		}

		public async Task<SearchResultModel[]> SearchAsync(string search, SearchParameters[] parameters)
		{
			var existingFactories = parameters
				.Join(this._factories, p => p.ProviderKey, f => f.Key, (p, f) => new
				{
					Factory = f,
					Pagination = p.Pagination,
				})
				.ToArray();

			if (existingFactories.Length < parameters.Length)
			{
				var notFound = parameters
					.Select(p => p.ProviderKey)
					.Except(existingFactories.Select(ef => ef.Factory.Key))
					.ToArray();

				throw new ProvidersNotFoundException(notFound);
			}

			var tasks = existingFactories
				.Select(ef => new
				{
					Key = ef.Factory.Key,
					Task = ef.Factory.Create()
						.SearchAsync(search, ef.Pagination),
				})
				.ToArray();

			try
			{
				await Task.WhenAll(tasks.Select(t => t.Task));
			}
			catch (Exception)
			{
				var tasksWithExceptions = tasks
					.Where(t => t.Task.IsFaulted)
					.ToArray();

				throw new AggregateException("Exception while performing search on the following providers: " +
					$"{String.Join(", ", tasksWithExceptions.Select(twe => twe.Key))}.",
					tasksWithExceptions.Select(twe => twe.Task.Exception));
			}

			return tasks
				.Select(t => new SearchResultModel(t.Key, t.Task.Result))
				.ToArray();
		}
	}
}
