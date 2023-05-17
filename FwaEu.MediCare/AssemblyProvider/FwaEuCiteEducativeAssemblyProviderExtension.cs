using FwaEu.MediCare.AssemblyProvider;
using FwaEu.Modules.AssemblyProvider;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.MediCare.AssemblyProvider
{
	public static class FwaEuMediCareAssemblyProviderExtension
	{
		public static IServiceCollection AddFwaEuMediCareAssemblyProviderExtension(this IServiceCollection services)
		{
			services.AddTransient<IAssemblyProvider, FwaEuMediCareAssemblyProvider>();
			return services;
		}
	}
}
