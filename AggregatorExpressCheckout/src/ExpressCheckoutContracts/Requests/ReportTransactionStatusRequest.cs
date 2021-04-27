using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Requests
{
   public class ReportTransactionStatusRequest
    {
        [JsonProperty("merchant_id")]
        public string m_iMerchantId { get; set; }
        [JsonProperty("transaction_status")]
        public bool m_bTransactionStatus { get; set; }
        [JsonProperty("transaction_id")]
        public string m_strTransactionId { get; set; }
    }
}
