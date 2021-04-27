using Newtonsoft.Json;

namespace ExpressCheckoutContracts.DTO.Razorpay
{
    public class RazorpayNetbankingRequestDto : RazorpayPaymentRequestDto
    {
        [JsonProperty("bank")]
        public string Bank { get; set; }
    }
}
