using Microsoft.Extensions.DependencyInjection;

namespace BussnissLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayerRegistration(this IServiceCollection services)
        {
            return services;
        }
    }
}
