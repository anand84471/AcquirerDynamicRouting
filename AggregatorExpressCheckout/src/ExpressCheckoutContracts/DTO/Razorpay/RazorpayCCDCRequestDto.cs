using Newtonsoft.Json;

namespace ExpressCheckoutContracts.DTO.Razorpay
{
    public class RazorpayCCDCRequestDto : RazorpayPaymentRequestDto
    {
        [JsonProperty("card")]
        public RazorpayCardDataDto Card { get; set; }
    }
}
