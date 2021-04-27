using Newtonsoft.Json;

namespace ExpressCheckoutContracts.Requests
{
    public class OrderDetailsRequest
    {

        [JsonProperty("merchant_data")]
        public MerchantRequest MerchantRequest { get; set; }

        [JsonProperty("payment_info_data")]
        public OrderTxnInfoRequest OrderTxnInfoRequest { get; set; }

        [JsonProperty("customer_data")]
        public CustomerRequest CustomerRequest { get; set; }

        [JsonProperty("billing_address_data")]
        public BillingAddressRequest BillingAddressRequest { get; set; }

        [JsonProperty("shipping_address_data")]
        public ShippingAddressRequest ShippingAddressRequest { get; set; }

        [JsonProperty("product_info_data")]
        public ProductInfoRequest ProductInfoRequest { get; set; }

        [JsonProperty("additional_info_data")]
        public AdditonalDataRequest AdditonalDataRequest { get; set; }

        [JsonProperty("card_data")]
        public CardRequest CardRequest { get; set; }
    }
}