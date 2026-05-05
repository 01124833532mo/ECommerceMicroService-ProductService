using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayerRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection")!;
            connectionString = connectionString.Replace("$MYSQL_HOST", Environment.GetEnvironmentVariable("MYSQL_HOST") ?? "localhost");
            connectionString = connectionString.Replace("$MYSQL_PASSWORD", Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "admin");
            connectionString = connectionString.Replace("$MYSQL_DATABASE", Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? "ecommerceproductsdatabase");
            connectionString = connectionString.Replace("$MYSQL_PORT", Environment.GetEnvironmentVariable("MYSQL_PORT") ?? "3306");
            connectionString = connectionString.Replace("$MYSQL_USER", Environment.GetEnvironmentVariable("MYSQL_USER") ?? "root");

            
            services.AddDbContext<Context.ApplicationDbContext>(options =>
                options.UseMySQL(connectionString));

            services.AddScoped<RepositoryContracts.IProductRepository, Repositories.ProductsRepository>();
            return services;
        }
    }
}
