using ExpressCheckoutContracts.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO.Routing.DtoExposeToMerchant
{
    public class PaymentDto
    {

        [JsonProperty("card_bin")]
        public string cardBin { get; set; }

        [JsonProperty("card_issuer")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumIssuer cardIssuer { get; set; }

        [JsonProperty("card_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumCardType cardType { get; set; }

        [JsonProperty("card_brand")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumCardScheme cardBrand { get; set; }

        [JsonProperty("payment_method")]
        public string paymentMethod { get; set; }

        [JsonProperty("payment_method_type")]
        public string paymentMethodType { get; set; }
    }
}
