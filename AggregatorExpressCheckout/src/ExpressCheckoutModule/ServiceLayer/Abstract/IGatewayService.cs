using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Response.Concrete;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ServiceLayer.Abstract
{
    public interface IGatewayService
    {
        IGatewayIntegrationHandlerService GetGatewayIntegrationHandlerService(EnumGateway enumGateway);

        Task<GatewayDto> GetGatewayDetails(int gateWayId);
        Task<DynamicRoutingGatewayResponse> GatewayOrderDetails(long AggOrderId);
        Task<List<int>> EnablePaymentModeList(int merchantId, int PaymentMode);
    }
}
