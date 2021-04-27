using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Concrete;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ExpressCheckout.BusinessLayer.Abstract
{
    public interface IOrderValidation
    {
        Task<long> CreatePurchaseOrder(IHeaderDictionary headers, AcceptOrderDetailsRequest acceptOrderDetailsRequest);

        Task<bool> UpdatePurchaseOrdrer(IHeaderDictionary headers, AcceptOrderDetailsRequest acceptOrderDetailsRequest);

        Task<string> DoPayment(long orderId, DoPaymentRequest doPaymentRequest);

        Task<OrderDetailsResponseSentToMerchant> DoInquiry(IHeaderDictionary headers, AcceptOrderDetailsRequest acceptOrderDetailsRequest);
        Task<OrderDetailsResponseSentToMerchant> DoRefund(IHeaderDictionary headers, AcceptOrderDetailsRequest acceptOrderDetailsRequest);
      
    }
}