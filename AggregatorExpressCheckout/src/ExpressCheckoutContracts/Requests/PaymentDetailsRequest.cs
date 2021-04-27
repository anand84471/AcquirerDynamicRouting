using ExpressCheckoutContracts.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExpressCheckoutContracts.Requests
{
    public class PaymentDetailsRequest
    {
        [JsonProperty("preferred_gateway")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumGateway PrefferedGatewayCode { get; set; }

        [JsonProperty("payment_mode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumPaymentMode PaymentMode { get; set; }
    }
}
