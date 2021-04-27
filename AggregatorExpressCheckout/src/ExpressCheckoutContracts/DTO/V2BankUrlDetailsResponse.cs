using ExpressCheckoutContracts.Response.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO
{
    public class V2BankUrlDetailsResponse: MasterResponse
    {
        [JsonProperty("url_data")]
        public string urlData;
    }
}
