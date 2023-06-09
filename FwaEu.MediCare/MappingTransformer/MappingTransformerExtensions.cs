
using FwaEu.Fwamework;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.MediCare.MappingTransformer
{
    public static class MappingTransformerExtensions
    {
        public static IServiceCollection AddApplicationMappingTransformer(this IServiceCollection services)
        {
            services.AddTransient<IMappingTransformer, DefaultMappingTransformer>();

            return services;
        }
    }
}
