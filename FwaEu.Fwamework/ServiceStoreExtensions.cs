using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework
{
	public static class ServiceStoreExtensions
	{
		public static TService GetOrAdd<TService>(this ServiceStore serviceStore)
			where TService : new()
		{
			return serviceStore.GetOrAdd(() => new TService());
		}

		public static TService GetRequiredService<TService>(this ServiceStore serviceStore)
		{
			var service = serviceStore.Get<TService>();

			if (service == null)
			{
				throw new ApplicationException($"A service of type {typeof(TService)} is required.");
			}

			return service;
		}
	}
}
