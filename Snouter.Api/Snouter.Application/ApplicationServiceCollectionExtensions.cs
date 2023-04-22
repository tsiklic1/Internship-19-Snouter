﻿

using Blog.Application.Database;
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
            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddSingleton<ISubcategoryRepository, SubcategoryRepository>();
            services.AddSingleton<ISubcategoryService, SubcategoryService>();

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
