using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Concrete;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutModule.ApiClients.Abstract;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using ExpressCheckoutPaymentIntegrationModule.Factory.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ServiceLayer.Concrete
{
    public class GatewayService : IGatewayService
    {
        private readonly IPaymentIntegrationModuleFactory _paymentIntegrationModuleFactory = null;
        private readonly IGatewayRepo _gatewayRepo;
        IPinePGApiClient _pinePGApiClient;
        public GatewayService(IPaymentIntegrationModuleFactory paymentIntegrationModuleFactory, IGatewayRepo gatewayRepo,IPinePGApiClient pinePGApiClient)
        {
            _paymentIntegrationModuleFactory = paymentIntegrationModuleFactory;
            _gatewayRepo = gatewayRepo;
            _pinePGApiClient = pinePGApiClient;

        }
        public Task<GatewayDto> GetGatewayDetails(int gateWayId)
        {
            return _gatewayRepo.GetGatewayDetails((EnumGateway)gateWayId);
        }

        public IGatewayIntegrationHandlerService GetGatewayIntegrationHandlerService(EnumGateway enumGateway)
        {
           return  _paymentIntegrationModuleFactory.GetPaymentGatewayHandlerService(enumGateway);

        }
        public async Task<DynamicRoutingGatewayResponse> GatewayOrderDetails(long AggOrderId)
        {
            DynamicRoutingGatewayResponse dynamicRoutingGatewayResponse;
            GatewayOrderDetailsRequest gatewayOrderDetailsRequest= new GatewayOrderDetailsRequest();
            gatewayOrderDetailsRequest.agg_Order_id = AggOrderId;
            dynamicRoutingGatewayResponse = await _pinePGApiClient.GetPaymentGatewayOrder(gatewayOrderDetailsRequest);
            return dynamicRoutingGatewayResponse;
        }

        public async Task<List<int>> EnablePaymentModeList(int merchantId,int PaymentMode)
        {
            EnableGatewayPaymentModeDto enableGatewayPaymentModeDto;
            enableGatewayPaymentModeDto = await _gatewayRepo.GetEnableGatewayList(merchantId, PaymentMode);
            return enableGatewayPaymentModeDto.EnableGateway;
        }

    }
}
