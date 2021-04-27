using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Requests.Routing
{
   public class AdditionalDataRequest
    {

        [JsonProperty("unix_time_stamp")]
        public int unxiTimeStamp { get; set; }
    }
}
