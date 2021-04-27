using Core.ApiClients;
using ExpressCheckoutContracts.ExternalApis.Requests;
using ExpressCheckoutContracts.ExternalApis.Responses;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Concrete;
using ExpressCheckoutModule.ApiClients.Abstract;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ApiClients.Concrete
{
    public class PinePGApiClient : IPinePGApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PinePGApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GlobabBinCardInfoResponse> GetCardInfoData(GlobalBinCardInfoRequest globalBinRequest)
        {
            GlobabBinCardInfoResponse globabBinCardInfoResponse = null;
            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient("globalBinRangeClient"))
                {
                    globabBinCardInfoResponse = await CoreApiClient.DoPostRequestJsonAsync<HttpClient, GlobalBinCardInfoRequest, GlobabBinCardInfoResponse>(client, "/api/GlobalBIN/GetBinData", globalBinRequest);
                }
            }
            catch (HttpRequestException httpRequestEx)
            {
            }
            catch (Exception ex)
            {
            }

            return globabBinCardInfoResponse;
        }


        public async Task<DynamicRoutingGatewayResponse> GetPaymentGatewayOrder(GatewayOrderDetailsRequest gatewayOrderDetailsRequest)
        {
            DynamicRoutingGatewayResponse dynamicRoutingGatewayResponse = null;
            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient("gatewayOrderClient"))
                {
                    dynamicRoutingGatewayResponse = await CoreApiClient.DoPostRequestJsonAsync<HttpClient, GatewayOrderDetailsRequest, DynamicRoutingGatewayResponse>(client, "api/Dynamic/routing", gatewayOrderDetailsRequest);
                }
            }
            catch (HttpRequestException httpRequestEx)
            {
            }
            catch (Exception ex)
            {
            }

            return dynamicRoutingGatewayResponse;
        }


    }
}