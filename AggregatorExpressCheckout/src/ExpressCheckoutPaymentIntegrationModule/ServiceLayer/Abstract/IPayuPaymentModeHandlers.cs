using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;
using System.Threading.Tasks;

namespace ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract
{
    public interface IPayuPaymentModeHandlers
    {
        Task<string> DoPayment(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetailsDto);
    }
}
