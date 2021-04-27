using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Response.Abstract;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Response.Concrete
{
  public class MerchantResponse : MasterResponse
    {

        [JsonProperty("payment_modes", NullValueHandling = NullValueHandling.Ignore, ItemConverterType = typeof(StringEnumConverter))]
        public EnumPaymentMode[] enumPaymentModes { get; set; }

    }

    
}
