using FwaEu.MediCare.GenericRepositorySession;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.MediCare.GenericSession
{
    public static class GenericSessionExtensions
    {
        public static IServiceCollection AddApplicationGenericSession(this IServiceCollection services)
        {
            services.AddTransient<GenericSessionContext>();
            return services;
        }

    }
}