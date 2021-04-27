using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutPaymentIntegrationModule.Factory.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Concrete.PayU;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Concrete.Razorpay;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Concrete.RazorPay;
using System;

namespace ExpressCheckoutPaymentIntegrationModule.Factory.Concrete
{
    public class PaymentIntegrationModuleFactory : IPaymentIntegrationModuleFactory
    {
        private readonly IServiceProvider serviceProvider;

        public PaymentIntegrationModuleFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IGatewayIntegrationHandlerService GetPaymentGatewayHandlerService(EnumGateway enumGateway)
        {
            switch (enumGateway)
            {
                case EnumGateway.RAZOR_PAY:
                    return (IGatewayIntegrationHandlerService)serviceProvider.GetService(typeof(RazorpayIntegrationHandlerService));
                case EnumGateway.PayU:
                    return (IGatewayIntegrationHandlerService)serviceProvider.GetService(typeof(PayuIntegrationHandlerService));
                default:
                    throw new OrderException(ResponseCodeConstants.PREFERRED_GATEWAY_IS_INVALID);
            }
        }

        public IPayuPaymentModeHandlers GetPayuPaymentModeHandlers(EnumPaymentMode enumPaymentMode)
        {
            switch (enumPaymentMode)
            {
                case EnumPaymentMode.CREDIT_DEBIT:
                    return (IPayuPaymentModeHandlers)serviceProvider.GetService(typeof(PayuCCDCHandlerService));
                case EnumPaymentMode.NETBANKING:
                    return (IPayuPaymentModeHandlers)serviceProvider.GetService(typeof(PayuNetbankingHandlerService));
                default:
                    throw new OrderException(ResponseCodeConstants.INVALID_PAYMENT_MODE);
            }
        }

        public IRazorpayPaymentModeHandlers GetRazorpayPaymentModeHandlerService(EnumPaymentMode enumPaymentMode) 
        {
            switch (enumPaymentMode)
            {
                case EnumPaymentMode.CREDIT_DEBIT:
                    return (IRazorpayPaymentModeHandlers)serviceProvider.GetService(typeof(RazorpayCCDCHandlerService));
                case EnumPaymentMode.NETBANKING:
                    return (IRazorpayPaymentModeHandlers)serviceProvider.GetService(typeof(RazorpayNetbankingHandlerService));
                default:
                    throw new OrderException(ResponseCodeConstants.INVALID_PAYMENT_MODE);
            }
        }
    }
}
