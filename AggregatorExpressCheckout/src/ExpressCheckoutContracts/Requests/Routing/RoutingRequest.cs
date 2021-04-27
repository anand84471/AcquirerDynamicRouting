using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Requests.Routing
{
   public class RoutingRequest
    {
        [JsonProperty("order")]
        public OrderRequest orderRequest { get; set; }

        [JsonProperty("txn")]
        public TxnRequest txnRequest { get; set; }

        [JsonProperty("payment")]
        public PaymentRequest paymentRequest { get; set; }

        [JsonProperty("additional_data")]
        public AdditionalDataRequest additionalDataRequest { get; set; }


    }
}
