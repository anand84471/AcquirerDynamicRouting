using Core.Infrastructure.DI;
using ExpressCheckoutDb.DBClients.Abstarct;
using ExpressCheckoutDb.DBClients.Abstract;
using ExpressCheckoutDb.DBClients.Concrete;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutDb.Repository.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressCheckoutDb.DI
{
    internal class ExpressCheckoutDBDependencyRegistration : IDependencyRegistrations
    {
        public void RegisterDependencies(IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services.AddTransient<ICustomerRepo, CustomerRepo>();
            services.AddTransient<ICustomerSavedCardRepo, CustomerSavedCardRepo>();
            services.AddTransient<IMerchantRepo, MerchantRepo>();
            services.AddTransient<IResponseCodeRepo, ResponseCodeRepo>();
            services.AddTransient<IDBServiceClient, DBServiceClient>();
            services.AddTransient(serviceProvider => serviceProvider.GetService<IDBServiceClient>()._AggregatorExpressCheckoutServiceClient);
            services.AddSingleton<DBServiceClientHttpEndPointBehaviour>();
            services.AddTransient<IOrderRepo, OrderRepo>();
            services.AddTransient<IGatewayRepo, GatewayRepo>();
            services.AddTransient<INetbankingRepo, NetbankingRepo>();
            services.AddTransient<IAndroidPGSdkDBRepo, PostgressAndroidSDKDBRepo>();
            services.AddTransient<IDynamicRoutingRepo, DynamicRoutingRepo>();
            services.AddTransient<IPostgressDBServiceClient, PostgressDBServiceClient>();
        }
    }
}