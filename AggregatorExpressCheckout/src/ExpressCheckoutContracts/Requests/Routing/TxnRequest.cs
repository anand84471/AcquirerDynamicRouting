using ExpressCheckoutContracts.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Requests.Routing
{
    public class TxnRequest
    {

        [JsonProperty("txn_id")]
        public long txnId { get; set; }

        [JsonProperty("payment_mode")]
        public EnumPaymentMode? paymentode { get; set; }
    }


}
