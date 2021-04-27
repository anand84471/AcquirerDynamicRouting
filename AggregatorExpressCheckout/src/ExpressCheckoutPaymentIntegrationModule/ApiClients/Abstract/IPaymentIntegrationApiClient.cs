using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.Razorpay;
using System.Threading.Tasks;

namespace ExpressCheckoutPaymentIntegrationModule.ApiClients.Abstract
{
    public interface IPaymentIntegrationApiClient
    {
        Task<RazorpayOrderResponseDto> CreateOrderAtRazorpay(RazorpayOrderRequestDto razorpayOrderRequestDto, OrderDetailsDto orderDetailsDto);

        Task<string> DoPaymentAtRazorpay(RazorpayPaymentRequestDto paymentRequestDto, MerchantGatewayConfigurationMappingDto merchantGatewayConfiguration, string url);

        Task<RazorPayInquiryResponseDto> DoInquiryOnRazorPay(string url, string apiUsername, string apiPassword);
       Task<RazorPayRefundResponseDto> DoRefundOnRazorPay(string url, RazorPayRefundRequestDto razorPayRefundRequestDto, string apiUsername, string apiPassword);
        Task<RazorPayRefundInquiryResponseDto> DoRefundInquiryOnRazorPay(string url, string apiUsername, string apiPassword);
    }
}
