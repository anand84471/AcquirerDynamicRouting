using Newtonsoft.Json;

namespace ExpressCheckoutContracts.Requests
{
    public class CustomerRequest
    {
        [JsonProperty("customer_id")]
        public long CustomerId { get; set; }

        public string CustomerReferenceNumber { get; set; }

        public long MerchantId { get; set; }

        [JsonProperty("customer_first_name")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [JsonProperty("mobile_no")]
        public string MobileNumber { get; set; }

        [JsonProperty("email_id")]
        public string EmailId { get; set; }

        public string CountryCode { get; set; }
    }
}