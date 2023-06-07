
using FwaEu.Fwamework;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.MediCare.MappingTransformer
{
    public static class MappingTransformerExtensions
    {
        public static IServiceCollection AddApplicationPatients(this IServiceCollection services)
        {
            services.AddTransient<IDatabaseMappingTransformer, DatabaseMappingTransformer>();

            return services;
        }
    }
}
