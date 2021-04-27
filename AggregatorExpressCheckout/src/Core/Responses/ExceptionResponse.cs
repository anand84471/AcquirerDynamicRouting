using Newtonsoft.Json;

namespace Core.Responses
{
    public class ExceptionResponse
    {
        [JsonProperty("response_code", NullValueHandling = NullValueHandling.Ignore)]
        public int Code { get; set; }

        [JsonProperty("response_message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}
