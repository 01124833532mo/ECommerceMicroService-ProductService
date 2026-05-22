using BussnissLogicLayer.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace BussnissLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayerRegistration(this IServiceCollection services)
        {
            services.AddScoped<ServiceContracts.IProductsService, Services.ProductsService>();
            services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();
            return services;
        }
    }
}
