using System;
using System.Collections.Generic;
using System.Text;
using ExpressCheckoutContracts.Response.Abstract;

namespace ExpressCheckoutContracts.Response.Concrete
{
    public class PayUPurchaseResponse : MasterResponse
    {
        public string OrderDetails { get; set; }
    }
}
