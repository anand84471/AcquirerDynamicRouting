using ExpressCheckoutContracts.Response.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Response.Concrete
{
   public class InquiryResponse : MasterResponse
    {

        public OrderDetailsResponseSentToMerchant OrderDetails { get; set; }

    }
}
