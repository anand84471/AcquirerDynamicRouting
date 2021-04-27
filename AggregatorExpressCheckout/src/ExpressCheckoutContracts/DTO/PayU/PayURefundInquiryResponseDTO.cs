using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ExpressCheckoutContracts.DTO.PayU
{
   public  class PayURefundInquiryResponseDTO
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        [JsonProperty("transaction_details")]
        public Dictionary<string,object> transactionDetails { get; set; }
    }


    public class InquiryTransactionDetails
    {


        [JsonProperty("bank_ref_num")]
        public string BankRefNum { get; set; }

        [JsonProperty("mihpayid")]
        public string GateWayPaymentId { get; set; }



        [JsonProperty("request_id")]
        public string RequestId { get; set; }
        [JsonProperty("amt")]
        public string Amt { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("bank_arn")]
        public string BankArn { get; set; }

        [JsonProperty("settlement_id")]
        public string SettlementId { get; set; }

        [JsonProperty("amount_settled")]
        public string AmountSettled { get; set; }


    }
}
