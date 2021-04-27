using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Response.Abstract;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Response.Concrete
{
    public class OrderDetailsResponseSentToMerchant:MasterResponse
    {
        [JsonProperty("merchant_data", NullValueHandling = NullValueHandling.Ignore)]
        public  MerchantDetailsResponse MerchantDetailsResponse { get; set; }

        [JsonProperty("order_data", NullValueHandling = NullValueHandling.Ignore)]
        public OrderDetailsResponse OrderDetailsResponse { get; set; }

        [JsonProperty("payment_info_data", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentTxnResponse PaymentTxnResponse { get; set; }

        [JsonIgnore]
        public string CustomHeader { get; set; }

    

    }

    public class MerchantDetailsResponse
    {
        [JsonProperty("merchant_id", NullValueHandling = NullValueHandling.Ignore)]
        public int MerchantId { get; set; }

        [JsonProperty("order_id", NullValueHandling = NullValueHandling.Ignore)]
        public string MerchantOrderID { get; set; }
    }

    public class OrderDetailsResponse
    {
        [JsonProperty("agg_order_id", NullValueHandling = NullValueHandling.Ignore)]
        public long AggOrderID { get; set; }

        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public long Amount { get; set; }

        [JsonProperty("currency_code", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumCurrency CurrencyCode { get; set; }

        [JsonProperty("order_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string OrderDesc { get; set; }

        [JsonProperty("transaction_type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumTxnType? TxnType { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumOrderStatus? OrderStatus { get; set; }

        [JsonProperty("response_code", NullValueHandling = NullValueHandling.Ignore)]
        public int OrderResponseCode { get; set; }

        [JsonProperty("refund_amount", NullValueHandling = NullValueHandling.Ignore)]
        public long? RefundAmount { get; set; }

        [JsonProperty("parent_order_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? ParentAggOrderId { get; set; }

        [JsonProperty("parent_status", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumOrderStatus? ParentOrderStatus { get; set; }

        [JsonProperty("parent_response_code", NullValueHandling = NullValueHandling.Ignore)]
        public int? ParentOrderResponseCode { get; set; }

    }

    public class PaymentTxnResponse
    {
       

        [JsonProperty("payment_mode",NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumPaymentMode? PaymentMode { get; set; }

        [JsonProperty("gateway_data", NullValueHandling = NullValueHandling.Ignore)]
        public GatewayDetailsResponse GatewayDetailsResponse { get; set; }

        [JsonProperty("acquirer_data", NullValueHandling = NullValueHandling.Ignore)]
        public AcquirerDataResponse AcquirerDataResponse { get; set; }
    }


    public class GatewayDetailsResponse
    {
        [JsonProperty("gateway")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumGateway GatewayCode { get; set; }

        [JsonProperty("order_id", NullValueHandling = NullValueHandling.Ignore)]
        public string OrderId { get; set; }

        [JsonProperty("payment_id", NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentId { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }



    }

    public class AcquirerDataResponse
    {
        [JsonProperty("acquirer", NullValueHandling = NullValueHandling.Ignore)]
        public string AcquirerName { get; set; }

    }
     

    /*
           * Inquiry
           *      Purchase:(parent order id)
               *      merchant data  (merchant id ,merchant order id)
               *      Order txn data(transaction type, agg order id,payment id,order status,order response code ,amount,refund amount)
               *      payment data (payment mode , gateway name,gateway payment id,gateway payment id,gatway status)
               *           acquirer :bank txn,auth rrn
               `
           *   Inquiry:
           *       Refund:(parent order id)
           *        merchant data  (merchant id ,merchant order id)
               *      Order txn data(transaction type, agg order id,payment id,order status,order response code ,amount,refund amount,parent agg order id,parent order response code,parent response status)
               *      payment data (gateway name,gateway payment id,gateway payment id,gatway status,)
               *           acquirer :bank txn,auth rrn
           *            
           *   
           *      
           * 
           * 
           * 
           */

}
