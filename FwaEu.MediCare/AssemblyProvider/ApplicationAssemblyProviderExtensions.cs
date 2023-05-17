using FwaEu.Modules.AssemblyProvider;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.MediCare.AssemblyProvider
{
	public static class ApplicationAssemblyProviderExtensions
	{
		public static IServiceCollection AddApplicationAssemblyProvider(this IServiceCollection services)
		{
			services.AddTransient<IAssemblyProvider, ApplicationAssemblyProvider>();
			return services;
		}
	}
}
