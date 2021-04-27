using Core.Infrastructure.DI;
using ExpressCheckoutModule.ApiClients.Abstract;
using ExpressCheckoutModule.ApiClients.Concrete;
using ExpressCheckoutModule.Cache.Abstract;
using ExpressCheckoutModule.Cache.Concrete;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using ExpressCheckoutModule.ServiceLayer.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ExpressCheckoutModule.DI
{
    public class ExpressCheckoutModuleDependencyRegistration : IDependencyRegistrations
    {
        public void RegisterDependencies(IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services.AddSingleton<IExpressCheckoutCache, ExpressCheckoutCache>();
            services.AddHttpClient("globalBinRangeClient", client =>
            {
                client.BaseAddress = new Uri("http://192.168.101.205:9059");
               
            });
            services.AddHttpClient("gatewayOrderClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:44339");

            });
            services.AddTransient<IMerchantService, MerchantService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ICustomerSavedCardService, CustomerSavedCardService>();
            services.AddTransient<IPinePGApiClient, PinePGApiClient>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IGatewayService, GatewayService>();
            services.AddTransient<IDynamicRoutingService, DynamicRoutingService>();
            services.AddTransient<IAndroidSDKService, AndroidSDKService>();
            services.AddTransient<IGlobalBinDataService, GlobalBinDataService>();

        }
    }
}