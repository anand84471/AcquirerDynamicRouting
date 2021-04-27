using Newtonsoft.Json;

namespace ExpressCheckoutContracts.DTO.Razorpay
{
    public abstract class RazorpayPaymentRequestDto
    {
        [JsonProperty("key_id")]
        public string KeyId { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("contact")]
        public string Contact { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("callback_url")]
        public string CallbackUrl { get; set; }
    }
}
