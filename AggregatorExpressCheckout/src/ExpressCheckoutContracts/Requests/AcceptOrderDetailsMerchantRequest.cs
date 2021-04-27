using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Requests
{
    public class DynamicRoutingDetailsRequest
    {
        [JsonProperty("agg_order_id")]
        public long AggOrderId { get; set; }


    }
}
