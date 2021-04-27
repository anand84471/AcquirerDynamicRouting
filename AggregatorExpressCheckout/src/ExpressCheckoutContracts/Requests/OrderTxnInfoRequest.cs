using ExpressCheckoutContracts.Enums;
using Newtonsoft.Json;

namespace ExpressCheckoutContracts.Requests
{
    public class OrderTxnInfoRequest : PaymentDetailsRequest
    {


        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("currency_code")]
        public EnumCurrency CurrencyCode { get; set; }

        [JsonProperty("order_desc")]
        public string OrderDesc { get; set; }

        [JsonProperty("parent_order_id")]
        public long ParentOrderId { get; set; }

        [JsonProperty("agg_order_id")]
        public long AggOrderId { get; set; }


       
    }
}