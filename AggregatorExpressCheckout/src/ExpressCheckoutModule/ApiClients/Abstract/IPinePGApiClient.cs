using ExpressCheckoutContracts.ExternalApis.Requests;
using ExpressCheckoutContracts.ExternalApis.Responses;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Concrete;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ApiClients.Abstract
{
    public  interface IPinePGApiClient
    {
        Task<GlobabBinCardInfoResponse> GetCardInfoData(GlobalBinCardInfoRequest globalBinRequest);
        Task<DynamicRoutingGatewayResponse> GetPaymentGatewayOrder(GatewayOrderDetailsRequest gatewayOrderDetailsRequest);
    }
}