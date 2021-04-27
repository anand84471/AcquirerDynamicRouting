using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure
{
    public static class WireUp
    {
        /// <summary>
        /// Adds the application settings.
        /// </summary>
        /// <param name="environmentName">Name of the environment.</param>
        /// <returns>Configuration builder</returns>
        public static IConfigurationBuilder AddAppSettings(string environmentName)
        {
            const string appSettingsFileName = "appsettings";
            return new ConfigurationBuilder()
                .AddJsonFile($"{appSettingsFileName}.json", false, true)
                .AddJsonFile($"{appSettingsFileName}.{environmentName}.json", true, true)
                .AddEnvironmentVariables();
        }

        public static void WireUpCore(IServiceCollection services)
        {
            services.AddOptions();
        }
    }
}