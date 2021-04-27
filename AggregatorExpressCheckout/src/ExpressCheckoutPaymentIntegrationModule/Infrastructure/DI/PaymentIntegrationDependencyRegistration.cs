using Core.Infrastructure.DI;
using ExpressCheckoutPaymentIntegrationModule.ApiClients.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ApiClients.Concrete;
using ExpressCheckoutPaymentIntegrationModule.Factory.Abstract;
using ExpressCheckoutPaymentIntegrationModule.Factory.Concrete;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Concrete.Common;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Concrete.PayU;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Concrete.Razorpay;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Concrete.RazorPay;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressCheckoutPaymentIntegrationModule.Infrastructure.DI
{
    public class PaymentIntegrationDependencyRegistration : IDependencyRegistrations
    {
        public void RegisterDependencies(IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services.AddHttpClient();

            services.AddTransient<IPaymentIntegrationApiClient, PaymentIntegrationApiClient>();

            services.AddTransient<IPaymentIntegrationModuleFactory, PaymentIntegrationModuleFactory>();

            services.AddTransient<ICommonIntegrationHandlerService, CommonIntegrationHandlerService>();


            services.AddTransient<RazorpayIntegrationHandlerService>()
                .AddTransient<IGatewayIntegrationHandlerService, RazorpayIntegrationHandlerService>(s => s.GetService<RazorpayIntegrationHandlerService>());

            services.AddTransient<PayuIntegrationHandlerService>()
                .AddTransient<IGatewayIntegrationHandlerService, PayuIntegrationHandlerService>(s => s.GetService<PayuIntegrationHandlerService>());

            services.AddTransient<RazorpayCCDCHandlerService>()
                .AddTransient<IRazorpayPaymentModeHandlers, RazorpayCCDCHandlerService>(s => s.GetService<RazorpayCCDCHandlerService>());

            services.AddTransient<RazorpayNetbankingHandlerService>()
                .AddTransient<IRazorpayPaymentModeHandlers, RazorpayNetbankingHandlerService>(s => s.GetService<RazorpayNetbankingHandlerService>());

            services.AddTransient<PayuCCDCHandlerService>()
                .AddTransient<IPayuPaymentModeHandlers, PayuCCDCHandlerService>(s => s.GetService<PayuCCDCHandlerService>());

            services.AddTransient<PayuNetbankingHandlerService>()
                    .AddTransient<IPayuPaymentModeHandlers, PayuNetbankingHandlerService>(s => s.GetService<PayuNetbankingHandlerService>());


        }
    }
}
