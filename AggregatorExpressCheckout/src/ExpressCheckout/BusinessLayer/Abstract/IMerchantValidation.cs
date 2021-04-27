using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests;
using System.Threading.Tasks;

namespace ExpressCheckout.BusinessLayer.Abstract
{
    public interface IMerchantValidation
    {
        Task<EnumPaymentMode[]> GetPaymentMode(OrderDetailsRequest orderDetailsRequest);
      
    }
}
