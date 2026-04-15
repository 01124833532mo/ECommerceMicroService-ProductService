using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayerRegistration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<Context.ApplicationDbContext>(options =>
                options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!));

            services.AddScoped<RepositoryContracts.IProductRepository, Repositories.ProductsRepository>();
            return services;

        }
    }
}
