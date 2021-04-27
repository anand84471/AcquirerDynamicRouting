using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.Razorpay;
using ExpressCheckoutContracts.Requests;
using System.Threading.Tasks;

namespace ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract
{
    public interface IRazorpayPaymentModeHandlers
    {
        Task<string> DoPayment(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetailsDto, RazorpayOrderResponseDto razorpayOrderResponseDto);
    }
}
