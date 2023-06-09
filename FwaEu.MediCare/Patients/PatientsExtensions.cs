using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.MediCare.Patients
{
    public static class PatientsExtensions
    {
        public static IServiceCollection AddApplicationPatients(this IServiceCollection services)
        {
            return services;
        }
    }
}
