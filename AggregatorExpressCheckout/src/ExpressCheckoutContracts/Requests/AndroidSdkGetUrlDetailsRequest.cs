using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ExpressCheckoutContracts.Requests
{
    public class AndroidSdkGetUrlDetailsRequest
    {
        [JsonProperty("client_type_id")]       
        public int iClientId { get; set; }
        [JsonProperty("is_required_net_banking_details")]
        public bool bIsRequiredNetBankingDetails { get; set; }
    }
}
