

using Blog.Application.Database;
using Microsoft.Extensions.DependencyInjection;
using Snouter.Application.Repository;

namespace Snouter.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            //services.AddSingleton<IPostService, PostService>(); product 

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services,
    string connectionString)
        {
            services.AddSingleton<IDbConnectionFactory>(_ =>
                new NpgsqlConnectionFactory(connectionString));
            services.AddSingleton<DbInitializer>();

            return services;
        }
    }
}
