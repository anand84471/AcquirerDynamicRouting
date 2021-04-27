using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace ExpressCheckoutContracts.DTO.Razorpay
{
   public class RazorPayInquiryResponseDto
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("amount")]
        public int Amount { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("card_id")]
        public string CardId { get; set; }
        [JsonProperty("entity")]
        public string Entity { get; set; }
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
        [JsonProperty("invoice_id")]
        public string InvoiceId { get; set; }
        [JsonProperty("international")]
        public bool? InternationalPayment { get; set; }

        [JsonProperty("amount_refunded")]
        public string AmountRefunded { get; set; }

        [JsonProperty("refund_status")]
        public string Refund_status { get; set; }

        [JsonProperty("captured")]
        public bool? Captured { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("bank")]
        public string BankName { get; set; }
        [JsonProperty("wallet")]
        public string Wallet { get; set; }

        [JsonProperty("vpa")]
        public string Vpa { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("contact")]
        public string Contact { get; set; }
        [JsonProperty("notes")]
        public List<NotesDetails> notes { get; set; }

        [JsonProperty("fee")]
        public string Fee { get; set; }

        [JsonProperty("tax")]
        public string Tax { get; set; }

        [JsonProperty("error_code")]
        public string Error_code { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
        [JsonProperty("created_at")]
        public string Created_at { get; set; }
    }
}
