using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.MediCare.GenericRepositorySession
{
    public static class GenericRepositorySessionExtensions
    {
        public static IServiceCollection AddApplicationGenericRepositorySession(this IServiceCollection services)
        {
            services.AddTransient<IGenericRepositorySessionService, GenericRepositorySessionService>();
            return services;
        }

    }
}