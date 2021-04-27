using Core.Utilities;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Response.Abstract;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExpressCheckoutContracts.Requests
{
    public class CardRequest : MasterResponse
    {
        [JsonProperty("card_number")]
        public string CardNumber { get; set; }

        [JsonProperty("expiry_year")]
        public string CardExpiryYear { get; set; }

        [JsonProperty("expiry_month")]
        public string CardExpiryMonth { get; set; }

        [JsonProperty("card_holder_name")]
        public string CardHolderName { get; set; }

        [JsonProperty("cvv", NullValueHandling = NullValueHandling.Ignore)]
        public string CVV { get; set; }

        [JsonProperty("saved_card_id", NullValueHandling = NullValueHandling.Ignore)]
        public long SavedCardId { get; set; }

        [JsonProperty("assosiciation_type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumCardScheme AssociationType { get; set; }

        [JsonProperty("issuer_id", NullValueHandling = NullValueHandling.Ignore)]
      
        public int IssuerId { get; set; }

        [JsonProperty("masked_card_number", NullValueHandling = NullValueHandling.Ignore)]
        public string MaskedCardNumber

        {
            get
            {
                return GenericUtility.MaskCardNumber(CardNumber);
            }
        }

        public bool ShouldSerializeCardNumber()
        {
            return false;
        }

        [JsonProperty("save_to_locker", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsCardToBeSave { get; set; }

        [JsonProperty("card_status", NullValueHandling = NullValueHandling.Ignore)]
        public int status { get; set; }

        [JsonProperty("card_holder_mobile_no")]
        public string CardHolderMobileNo { get; set; }

        [JsonProperty("issuer_name")]
        public string IssuerName { get; set; }

        [JsonProperty("card_type_id")]
        public int CardTypeId { get; set; }
    }
}