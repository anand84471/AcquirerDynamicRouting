using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO.Razorpay
{
  public  class RazorPayRefundResponseDto
    {

        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("entity")]
        public string Entity { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("receipt")]
        public string Receipt { get; set; }

        [JsonProperty("notes")]
        public List<NotesDetails> Notes { get; set; }

        [JsonProperty("created_at")]
        public string created_at { get; set; }

        [JsonProperty("acquirer_data")]
        public AcquirerData Acquirer_data { get; set; }

        [JsonProperty("payment_id")]
        public string PaymentId { get; set; }

        [JsonProperty("speed_processed")]
        public string Speed_processed { get; set; }

        [JsonProperty("speed_requested")]
        public string Speed_requested { get; set; }
    }
}
