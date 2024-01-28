using FwaEu.ElderAid.GenericRepositorySession;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.ElderAid.GenericSession
{
    public static class GenericSessionExtensions
    {
        public static IServiceCollection AddApplicationGenericSession(this IServiceCollection services)
        {

            services.AddSingleton<IManageGenericDbService, ManageGenericDbService>();
            services.AddTransient<GenericSessionContext>();
            return services;
        }

    }
}