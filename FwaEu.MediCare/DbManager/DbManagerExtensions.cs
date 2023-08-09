using FwaEu.MediCare.DbManager;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.MediCare.GenericRepositorySession
{
    public static class DbManagerExtensions
    {
        public static IServiceCollection AddApplicationDbManager(this IServiceCollection services)
        {
            services.AddTransient<IDbManagerService, DbManagerService>();
            return services;
        }

    }
}