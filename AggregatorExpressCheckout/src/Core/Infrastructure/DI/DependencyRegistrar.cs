using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Core.Infrastructure.DI
{
    public static class DependencyRegistrar
    {
        public static void RegisterAssemblies(IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            var diRegistrationType = typeof(IDependencyRegistrations);
            var registrationTypes = AppDomain.CurrentDomain.GetAssemblies()
.SelectMany(s => s.GetTypes())
.Where(p => diRegistrationType.IsAssignableFrom(p) && p.IsClass);
            foreach (var registrationType in registrationTypes)
            {
                var instance = Activator.CreateInstance(registrationType) as IDependencyRegistrations;
                instance?.RegisterDependencies(services, configurationRoot);
            }
        }
    }
}