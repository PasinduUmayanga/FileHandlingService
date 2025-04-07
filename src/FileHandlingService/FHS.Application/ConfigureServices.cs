using FHS.Application.Abstractions;
using FHS.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FHS.Application
{
    public static class ConfigureServices
    {

        /// <summary>
        /// This method is used to register the application services in the dependency injection container.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFileServices, FileServices>();
            return services;
        }
    }
}
