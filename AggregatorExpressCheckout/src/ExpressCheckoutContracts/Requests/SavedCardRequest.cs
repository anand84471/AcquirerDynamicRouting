using Newtonsoft.Json;

namespace ExpressCheckoutContracts.Requests
{
    public class SavedCardRequest
    {
        [JsonProperty("merchant_data")]
        public MerchantRequest merchantRequest { get; set; }

        [JsonProperty("customer_data")]
        public CustomerRequest customerRequest { get; set; }

        [JsonProperty("card_data")]
        public CardRequest cardRequest { get; set; }
    }
}