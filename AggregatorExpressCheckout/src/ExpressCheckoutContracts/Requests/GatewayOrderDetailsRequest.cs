using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Requests
{
   public class GatewayOrderDetailsRequest
    {
        [JsonProperty("agg_order_id")]
        public long agg_Order_id { get; set; }
    }
}
