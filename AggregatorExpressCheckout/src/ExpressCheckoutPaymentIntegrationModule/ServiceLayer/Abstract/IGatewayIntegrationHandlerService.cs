using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;
using System.Threading.Tasks;

namespace ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract
{
    public interface IGatewayIntegrationHandlerService
    {
        Task<string> DoPurchase(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetailsDto);
        Task<InquiryResponseDto> DoInquiryOfPurchase(OrderDetailsDto orderDetailsDto,GatewayDto gatewayDto,MerchantGatewayConfigurationMappingDto merchantGatewayConfigurationMappingDto);
      
        Task<InquiryResponseDto> DoInquiryOfRefund(OrderDetailsDto parentOrderDetailsDto, GatewayDto gatewayDto, MerchantGatewayConfigurationMappingDto merchantGatewayConfigurationMappingDto);
        Task<RefundResponseDto> DoRefund(OrderDetailsDto parentOrderDetailsDto, long AmountToBeRefunded, GatewayDto gatewayDto, MerchantGatewayConfigurationMappingDto merchantGatewayConfigurationMappingDto);
    }
}
