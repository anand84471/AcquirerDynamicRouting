using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Response.Abstract;

namespace ExpressCheckoutContracts.Response.Concrete
{
    public class RazorpayPurchaseResponse : MasterResponse
    {
        public string OrderDetails { get; set; }
    }
}
