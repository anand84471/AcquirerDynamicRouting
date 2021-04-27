using Newtonsoft.Json;

namespace ExpressCheckoutContracts.DTO.Razorpay
{
    public class RazorpayOrderResponseDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
