using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Requests
{
    public class DependentOrderRequest
    {

        [JsonProperty("merchant_data")]
        public MerchantRequest merchantRequest { get; set; }

        [JsonProperty("payment_info_data")]
        public OrderTxnInfoRequest orderTxnInfoRequest { get; set; }
    }
}
