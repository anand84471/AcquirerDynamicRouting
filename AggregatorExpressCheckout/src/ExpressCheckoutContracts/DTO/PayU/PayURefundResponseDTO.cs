using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ExpressCheckoutContracts.DTO.PayU
{
    public class PayURefundResponseDTO
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }


        [JsonProperty("request_id")]
        public string RequestId { get; set; }
        [JsonProperty("bank_ref_num")]
        public string BankRefNum { get; set; }

      
        [JsonProperty("mihpayid")]
        public string GateWayPaymentId { get; set; }


        [JsonProperty("error_code")]
        public string ErrorCode { get; set; }
    }
}
