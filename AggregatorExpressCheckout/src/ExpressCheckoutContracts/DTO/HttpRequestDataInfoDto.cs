using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO
{
    public class HttpRequestDataInfoDto
    {
      
        public string XVerify { get; set; }

        /// <summary>
        /// Gets or sets the x redirect method.
        /// </summary>
        /// <value>
        /// The x redirect method.
        /// </value>
    
        public string XRedirectMethod { get; set; }

        /// <summary>
        /// Gets or sets the x call back URL.
        /// </summary>
        /// <value>
        /// The x call back URL.
        /// </value>
      
        public string XCallBackUrl { get; set; }

        /// <summary>
        /// Gets or sets the x call method.
        /// </summary>
        /// <value>
        /// The x call method.
        /// </value>
        public string XCallMethod { get; set; }
    }
}