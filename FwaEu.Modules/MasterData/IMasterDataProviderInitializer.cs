using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData
{
	public interface IMasterDataProviderInitializer
	{
		string Key { get; }
		IServiceCollection ServiceCollection { get; }
	}


	public class MasterDataProviderInitializer : IMasterDataProviderInitializer
	{
		public string Key { get; }
		public IServiceCollection ServiceCollection { get; }

		public MasterDataProviderInitializer(string key, IServiceCollection serviceCollection)
		{
			Key = key;
			ServiceCollection = serviceCollection;
		}
	}
}
