using Core.Cache.Abstract;
using Core.Cache.Concrete;
using Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.DI
{
    class ExpressCheckoutCoreRegistration : IDependencyRegistrations
    {
        public void RegisterDependencies(IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services.AddSingleton<ICoreCache, CoreCache>();
            
        }
    }
}
