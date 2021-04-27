using ExpressCheckoutContracts.Enums;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;

namespace ExpressCheckoutPaymentIntegrationModule.Factory.Abstract
{
    public interface IPaymentIntegrationModuleFactory
    {
        IGatewayIntegrationHandlerService GetPaymentGatewayHandlerService(EnumGateway enumGateway);

        IRazorpayPaymentModeHandlers GetRazorpayPaymentModeHandlerService(EnumPaymentMode enumPaymentMode);

        IPayuPaymentModeHandlers GetPayuPaymentModeHandlers(EnumPaymentMode enumPaymentMode);
    }
}
