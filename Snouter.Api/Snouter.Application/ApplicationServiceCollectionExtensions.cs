

using Blog.Application.Database;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Snouter.Application.Repository;
using Snouter.Application.Services;

namespace Snouter.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IProductService, ProductService>();

            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<ICategoryService, CategoryService>(); 

            services.AddSingleton<ISubcategoryRepository, SubcategoryRepository>();
            services.AddSingleton<ISubcategoryService, SubcategoryService>();

            services.AddSingleton<IUserRepository, UserRepository>();   
            services.AddSingleton<IUserService, UserService>();

            services.AddSingleton<ISpecService, SpecService>();
            services.AddSingleton<ISpecRepository, SpecRepository>();

            services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);

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
