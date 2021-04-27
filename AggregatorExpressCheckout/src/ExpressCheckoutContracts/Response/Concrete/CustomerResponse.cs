using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Response.Abstract;
using Newtonsoft.Json;

namespace ExpressCheckoutContracts.Response.Concrete
{
    public class CustomerResponse : MasterResponse
    {
        [JsonProperty("customer_id")]
        public long CustomerId { get; set; }

        [JsonProperty("customer_ref_no")]
        public string CustomerReferenceNumber { get; set; }

        [JsonProperty("merchant_id")]
        public int MerchantId { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("mobile_number")]
        public string MobileNumber { get; set; }

        [JsonProperty("email_id")]
        public string EmailId { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
    }
}