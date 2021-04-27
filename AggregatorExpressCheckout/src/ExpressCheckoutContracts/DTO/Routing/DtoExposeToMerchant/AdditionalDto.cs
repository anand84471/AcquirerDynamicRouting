using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO.Routing.DtoExposeToMerchant
{
   public class AdditionalDto
    {

        [JsonProperty("unix_time_stamp")]
        public long unxiTimeStamp { get; set; }

    }
}
