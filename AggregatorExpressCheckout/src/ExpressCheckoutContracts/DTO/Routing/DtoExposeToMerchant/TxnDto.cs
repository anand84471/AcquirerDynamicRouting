using ExpressCheckoutContracts.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO.Routing.DtoExposeToMerchant
{
    public class TxnDto
    {

        [JsonProperty("txn_id")]
        public long txnId { get; set; }

        [JsonProperty("payment_mode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumPaymentMode paymentode { get; set; }
    }


}
