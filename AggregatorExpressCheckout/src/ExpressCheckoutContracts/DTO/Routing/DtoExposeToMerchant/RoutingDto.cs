using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO.Routing.DtoExposeToMerchant
{
   public class RoutingDto
    {
        [JsonProperty("order")]
        public OrderDto orderDto { get; set; }

        [JsonProperty("txn")]
        public PaymentDto txnDto { get; set; }

        [JsonProperty("payment")]
        public TxnDto paymentDto { get; set; }

        [JsonProperty("additional_data")]
        public AdditionalDto additionalDataDto { get; set; }


    }
}
