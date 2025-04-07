using Microsoft.Extensions.DependencyInjection;

namespace FHS.Domain
{
    public static class ConfigureServices
    {

        /// <summary>
        /// This method is used to register the domain services in the dependency injection container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
