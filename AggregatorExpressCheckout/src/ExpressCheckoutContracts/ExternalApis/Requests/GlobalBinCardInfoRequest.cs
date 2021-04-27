using System.Collections.Generic;

namespace ExpressCheckoutContracts.ExternalApis.Requests
{
    public class GlobalBinCardInfoRequest
    {
        public List<int> bins { get; set; }
    }
}