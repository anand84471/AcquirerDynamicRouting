using Newtonsoft.Json;

namespace ExpressCheckoutContracts.DTO.Razorpay
{
    public class RazorpayOrderRequestDto
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("receipt")]
        public string Receipt { get; set; }

        [JsonProperty("payment_capture")]
        public bool PaymentCapture => true;

    }
}
