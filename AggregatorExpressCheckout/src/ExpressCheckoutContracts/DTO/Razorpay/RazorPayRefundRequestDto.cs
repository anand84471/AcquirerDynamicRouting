using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO.Razorpay
{
  public  class RazorPayRefundRequestDto
    {
        [JsonProperty("amount")]
        public long amount { get; set; }
    }
}
