using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Requests
{
    public class ProductInfoRequest
    {
        [JsonProperty("product_details")]
        public ProductDetailsRequest[] productDetails { get; set; }
    }
}