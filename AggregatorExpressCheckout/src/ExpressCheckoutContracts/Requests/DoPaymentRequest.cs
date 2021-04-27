using ExpressCheckoutContracts.DTO;
using Newtonsoft.Json;

namespace ExpressCheckoutContracts.Requests
{
    public class DoPaymentRequest
    {
        [JsonProperty("merchant_details_data")]
        public MerchantRequest MerchantDetailsRequest { get; set; }

        [JsonProperty("payment_details_data")]
        public PaymentDetailsRequest PaymentDetailsRequest { get; set; }

        [JsonProperty("card_data")]
        public CardRequest CardRequest { get; set; }

        [JsonProperty("netbanking_data")]
        public NetbankingRequest NetbankingRequest { get; set; }
    }
}
