using Core.ApiClients;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.Razorpay;
using ExpressCheckoutPaymentIntegrationModule.ApiClients.Abstract;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExpressCheckoutPaymentIntegrationModule.ApiClients.Concrete
{
    public class PaymentIntegrationApiClient : IPaymentIntegrationApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PaymentIntegrationApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<RazorpayOrderResponseDto> CreateOrderAtRazorpay(RazorpayOrderRequestDto razorpayOrderRequestDto, OrderDetailsDto orderDetailsDto)
        {
            RazorpayOrderResponseDto razorpayOrderResponseDto = null;
            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient("razorPayApiClient"))
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", orderDetailsDto.MerchantGatewayConfigurationMappingDto.MerchantIdIssuedByGatewayToMerchant, orderDetailsDto.MerchantGatewayConfigurationMappingDto.PasswordIssuedByGatewayToMerchant))));

                    razorpayOrderResponseDto = await CoreApiClient.DoPostRequestJsonAsync<HttpClient, RazorpayOrderRequestDto, RazorpayOrderResponseDto>(client, orderDetailsDto.GatewayDto.OrderUrl, razorpayOrderRequestDto);
                }
            }
            catch (HttpRequestException httpRequestEx)
            {
            }
            catch (Exception ex)
            {
            }

            return razorpayOrderResponseDto;
        }

        public async Task<string> DoPaymentAtRazorpay(RazorpayPaymentRequestDto paymentRequestDto, MerchantGatewayConfigurationMappingDto merchantGatewayConfiguration, string url)
        {
            var razorPayCcdcResponse = string.Empty;
            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient("razorPayApiClient"))
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", merchantGatewayConfiguration.MerchantIdIssuedByGatewayToMerchant, merchantGatewayConfiguration.PasswordIssuedByGatewayToMerchant))));
                    razorPayCcdcResponse = await CoreApiClient.DoPostRequestJsonAndGetStringAsync<HttpClient, RazorpayPaymentRequestDto>(client, "https://api.razorpay.com/v1/payments", paymentRequestDto);
                }
            }
            catch (HttpRequestException httpRequestEx)
            {
            }
            catch (Exception ex)
            {
            }

            return razorPayCcdcResponse;
        }

        public async Task<RazorPayInquiryResponseDto> DoInquiryOnRazorPay(string url, string apiUsername, string apiPassword)
        {
            RazorPayInquiryResponseDto razorPayInquiryResponseDto = null;
            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient("razorPayApiClient"))
                {
                    razorPayInquiryResponseDto = await CoreApiClient.DoGetAsync<HttpClient, RazorPayInquiryResponseDto>(client, url, apiUsername, apiPassword);
                }
            }
            catch (HttpRequestException httpRequestEx)
            {
            }
            catch (Exception ex)
            {
            }

            return razorPayInquiryResponseDto;
        }

        public async Task<RazorPayRefundResponseDto> DoRefundOnRazorPay(string url, RazorPayRefundRequestDto razorPayRefundRequestDto, string apiUsername, string apiPassword)
        {
            RazorPayRefundResponseDto razorPayRefundResponseDto = null;
            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient("razorPayApiClient"))
                {
                    razorPayRefundResponseDto = await CoreApiClient.DoPostRequestJsonAsyncWithCredential<HttpClient,RazorPayRefundRequestDto, RazorPayRefundResponseDto>(client, url, razorPayRefundRequestDto, apiUsername, apiPassword);
                }
            }
            catch (HttpRequestException httpRequestEx)
            {
            }
            catch (Exception ex)
            {
            }

            return razorPayRefundResponseDto;
        }

        public async Task<RazorPayRefundInquiryResponseDto> DoRefundInquiryOnRazorPay(string url, string apiUsername, string apiPassword)
        {
            RazorPayRefundInquiryResponseDto razorPayRefundInquiryResponseDto = null;
            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient("razorPayApiClient"))
                {
                    razorPayRefundInquiryResponseDto = await CoreApiClient.DoGetAsync<HttpClient, RazorPayRefundInquiryResponseDto>(client, url, apiUsername, apiPassword);
                }
            }
            catch (HttpRequestException httpRequestEx)
            {
            }
            catch (Exception ex)
            {
            }

            return razorPayRefundInquiryResponseDto;
        }
    }
}
