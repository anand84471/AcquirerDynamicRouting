using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO.Routing
{
   public class MerchantRoutingConfigDetailsDto
    {
        public long SimpleRoutingConfigId { get; set; }
        public string SimpleRoutingLogicCSV { get; set; }
        public int SimpleRoutingPerferenceScore { get; set; }
        public long CustomizedRoutingConfigId { get; set; }
        public string CustomizedRoutingJSLogicInBase64EncodedForm { get; set; }
        public int CustomizedRoutingPerferenceScore { get; set; }
        public long SpecialRoutingConfigId { get; set; }
    }
}
