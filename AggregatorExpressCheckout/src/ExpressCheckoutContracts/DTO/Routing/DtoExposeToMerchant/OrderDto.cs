using ExpressCheckoutContracts.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO.Routing.DtoExposeToMerchant
{
   public class OrderDto
    {
        [JsonProperty("order_id")]
        public long orderId { get; set; }

        [JsonProperty("amount_in_paise")]
        public long amount { get; set; }

        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumCurrency currency{get;set;}

        [JsonProperty("preferred_gateway")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumGateway preferredGateway { get; set; }

        [JsonProperty("udf1")]
        public string UDF1 { get; set; }

        [JsonProperty("udf2")]
        public string UDF2 { get; set; }

        [JsonProperty("udf3")]
        public string UDF3 { get; set; }

        [JsonProperty("udf4")]
        public string UDF4 { get; set; }

        [JsonProperty("udf5")]
        public string UDF5 { get; set; }

        [JsonProperty("udf6")]
        public string UDF6 { get; set; }

        [JsonProperty("udf7")]
        public string UDF7 { get; set; }

        [JsonProperty("udf8")]
        public string UDF8 { get; set; }

        [JsonProperty("udf9")]
        public string UDF9 { get; set; }

        [JsonProperty("udf10")]
        public string UDF10 { get; set; }

     


    }
}
