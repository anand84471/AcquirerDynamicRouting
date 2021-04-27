using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Response.Abstract;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Response.Concrete
{
   public class DynamicRoutingGatewayResponse : MasterResponse
    {
        
        public List<string> Priorties { get; set; }
        
    }
}
