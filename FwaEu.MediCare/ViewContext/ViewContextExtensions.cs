using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.MediCare.ViewContext
{
	public static class ViewContextExtensions
	{
		public static IServiceCollection AddApplicationViewContext(this IServiceCollection services)
		{
			services.AddScoped<IViewContextService, HttpHeaderViewContextService>();
            return services;
		}
	}
}