using Microsoft.Extensions.Hosting;

namespace FwaEu.MediCare.Application
{
    public static class ComposedEnvironmentExtensions
    {
        public static bool IsLikeDevelopment(this IHostEnvironment environment)
        {
            return environment.EnvironmentName.Contains(Environments.Development);
        }
    }
}
