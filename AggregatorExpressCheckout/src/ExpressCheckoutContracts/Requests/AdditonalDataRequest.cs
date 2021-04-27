using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Requests
{
    public class AdditonalDataRequest
    {
        [JsonProperty("rfu1")]
        public string RFU1 { get; set; }

        public string RFU2 { get; set; }
        public string RFU3 { get; set; }
        public string RFU4 { get; set; }
        public string RFU5 { get; set; }
        public string RFU6 { get; set; }
        public string RFU7 { get; set; }
        public string RFU8 { get; set; }
        public string RFU9 { get; set; }
        public string RFU10 { get; set; }
        public string RFU11 { get; set; }
        public string RFU12 { get; set; }
        public string RFU13 { get; set; }
        public string RFU14 { get; set; }
        public string RFU15 { get; set; }

        public string RFU16 { get; set; }
        public string RFU17 { get; set; }
        public string RFU18 { get; set; }

        public string RFU19 { get; set; }
        public string RFU20 { get; set; }
    }
}