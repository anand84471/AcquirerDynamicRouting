using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Requests
{
    public class ProductDetailsRequest
    {
        [JsonProperty("product_code")]
        public string ProductCode { get; set; }

        [JsonProperty("product_amount")]
        public long ProductAmount { get; set; }
    }
}