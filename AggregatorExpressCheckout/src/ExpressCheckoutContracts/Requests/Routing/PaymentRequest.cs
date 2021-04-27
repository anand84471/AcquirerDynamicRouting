using ExpressCheckoutContracts.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Requests.Routing
{
    public class PaymentRequest
    {

        [JsonProperty("card_bin")]
        public string cardBin { get; set; }

        [JsonProperty("card_issuer")]
        public EnumIssuer? cardIssuer { get; set; }

        [JsonProperty("card_type")]
        public EnumCardType? cardType { get; set; }

        [JsonProperty("card_brand")]
        public EnumCardScheme? cardBrand { get; set; }

        [JsonProperty("payment_method")]
        public string paymentMethod { get; set; }

        [JsonProperty("payment_method_type")]
        public string paymentMethodType { get; set; }
    }
}
