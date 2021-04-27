using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Abstract;
using Newtonsoft.Json;

namespace ExpressCheckoutContracts.Response.Concrete
{
    public class SaveCardResponse : MasterResponse
    {
        [JsonProperty("card_details", NullValueHandling = NullValueHandling.Ignore)]
        public CardRequest[] cardDetailsResponse { get; set; }
    }
}