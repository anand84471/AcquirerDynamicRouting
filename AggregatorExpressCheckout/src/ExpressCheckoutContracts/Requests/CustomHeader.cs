using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Requests
{
    public class CustomHeader
    {
        [JsonProperty("X-VERIFY")]
        public string XVerify { get; set; }

        /// <summary>
        /// Gets or sets the x redirect method.
        /// </summary>
        /// <value>
        /// The x redirect method.
        /// </value>
        [JsonProperty("X-REDIRECT-METHOD")]
        public string XRedirectMethod { get; set; }

        /// <summary>
        /// Gets or sets the x call back URL.
        /// </summary>
        /// <value>
        /// The x call back URL.
        /// </value>
        [JsonProperty("X-CALLBACK-URL")]
        public string XCallBackUrl { get; set; }

        /// <summary>
        /// Gets or sets the x call method.
        /// </summary>
        /// <value>
        /// The x call method.
        /// </value>
        [JsonProperty("X-CALL-METHOD")]
        public string XCallMethod { get; set; }
    }
}