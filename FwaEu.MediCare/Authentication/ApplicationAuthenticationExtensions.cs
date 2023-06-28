using FwaEu.Fwamework.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FwaEu.MediCare.Authentication
{
    public static class ApplicationAuthenticationExtensions
    {
        public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services)
        {
            services.RemoveAll<IPasswordHasher>()
                .AddTransient<IPasswordHasher, MD5PasswordHasher>();

            return services;
        }
    }
}
