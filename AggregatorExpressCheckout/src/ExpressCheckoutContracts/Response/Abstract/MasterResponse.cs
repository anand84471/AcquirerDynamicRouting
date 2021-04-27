using Newtonsoft.Json;

namespace ExpressCheckoutContracts.Response.Abstract
{
    public class MasterResponse
    {
        [JsonProperty("response_code", NullValueHandling = NullValueHandling.Ignore)]
        public int Code { get; set; }

        [JsonProperty("response_message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}