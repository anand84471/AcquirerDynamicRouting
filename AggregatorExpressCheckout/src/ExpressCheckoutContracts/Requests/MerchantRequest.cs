using Newtonsoft.Json;

namespace ExpressCheckoutContracts.Requests
{
    public class MerchantRequest
    {
        [JsonProperty("merchant_id")]
        public int MerchantId { get; set; }

        [JsonProperty("merchant_access_code")]
        public string MerchantAccessCode { get; set; }

        [JsonProperty("merchant_return_url")]
        public string MerchantReturnUrl { get; set; }

        [JsonProperty("order_id")]
        public string MerchantOrderID { get; set; }
    }
}