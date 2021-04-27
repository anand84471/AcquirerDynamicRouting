using ExpressCheckoutContracts.Response.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Response.Concrete
{
    public class OrderCreateResponse:MasterResponse
    {
        [JsonProperty("agg_order_id", NullValueHandling = NullValueHandling.Ignore)]
        public long AggOrderId { get; set; }
    }
}
