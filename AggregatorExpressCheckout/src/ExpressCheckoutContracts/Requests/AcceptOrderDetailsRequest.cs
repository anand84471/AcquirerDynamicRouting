using ExpressCheckoutContracts.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO
{
    public class AcceptOrderDetailsRequest
    {
        [JsonProperty("request")]
        public string Request { get; set; }

        [JsonIgnore]
        public CustomHeader CustomHeader { get; set; }



        /// <summary>
        /// Gets or sets the source ip address.
        /// </summary>
        /// <value>
        /// The source ip address.
        /// </value>

        [JsonIgnore]
        public string SourceIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>
        /// The user agent.
        /// </value>
         [JsonIgnore]
        public string UserAgent { get; set; }
    }
}
