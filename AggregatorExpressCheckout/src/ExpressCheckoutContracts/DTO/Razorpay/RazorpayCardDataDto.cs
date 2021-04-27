using Newtonsoft.Json;

namespace ExpressCheckoutContracts.DTO.Razorpay
{
    public class RazorpayCardDataDto
    {
        [JsonProperty("name")]
        public string CardHolderName { get; set; }

        [JsonProperty("number")]
        public string CardNumber { get; set; }

        [JsonProperty("cvv")]
        public string CVV { get; set; }

        [JsonProperty("expiry_year")]
        public string CardExpiryYear { get; set; }

        [JsonProperty("expiry_month")]
        public string CardExpiryMonth { get; set; }
    }
}
