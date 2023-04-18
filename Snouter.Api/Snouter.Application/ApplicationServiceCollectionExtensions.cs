

using Microsoft.Extensions.DependencyInjection;
using Snouter.Application.Repository;

namespace Snouter.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
