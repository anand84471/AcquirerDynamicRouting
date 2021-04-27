using Newtonsoft.Json;

namespace ExpressCheckoutContracts.DTO.Razorpay
{
    public class RazorpayFinalResponseMetaData
    {
        [JsonProperty("payment_id")]
        public string PaymentId { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }
    }
}
