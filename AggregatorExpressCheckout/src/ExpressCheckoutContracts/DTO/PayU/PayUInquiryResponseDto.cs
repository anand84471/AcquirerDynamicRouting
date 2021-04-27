using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ExpressCheckoutContracts.DTO.PayU
{
    public class PayUInquiryResponseDto
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        [JsonProperty("transaction_details")]
        public TransactionDetails transactionDetails { get; set; }


    }

    public class TransactionDetails
    {

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
        [JsonProperty("bank_ref_num")]
        public string BankRefNum { get; set; }

        [JsonProperty("net_amount")]
        public string NetAmount { get; set; }
        [JsonProperty("mihpayid")]
        public string GateWayPaymentId { get; set; }
        [JsonProperty("amt")]
        public string amt { get; set; }
        [JsonProperty("disc")]
        public string Disc { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("txnid")]
        public string GateWayTxnId { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("amount_paid")]
        public string AmountPaid { get; set; }

        [JsonProperty("discount")]
        public string Discount { get; set; }

        [JsonProperty("additional_charges")]
        public string AdditionalCharges { get; set; }

        [JsonProperty("unmappedstatus")]
        public string UnMappedStatus { get; set; }


    }



  
}
