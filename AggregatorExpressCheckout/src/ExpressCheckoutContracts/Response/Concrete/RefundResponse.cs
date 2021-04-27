using System;
using System.Collections.Generic;
using System.Text;
using ExpressCheckoutContracts.Response.Abstract;

namespace ExpressCheckoutContracts.Response.Concrete
{
    public class RefundResponse : MasterResponse
    {
        public OrderDetailsResponseSentToMerchant OrderDetails { get; set; }
    }
}
