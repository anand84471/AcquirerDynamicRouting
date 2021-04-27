using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.DI
{
    public interface IDependencyRegistrations
    {
        /// <summary>
        /// Registers the dependencies.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configurationRoot">configurationRoot</param>
        void RegisterDependencies(IServiceCollection services, IConfigurationRoot configurationRoot);
    }
}