using System;
using System.Collections.Generic;
using Core.Cache.Abstract;

namespace Core.Cache.Concrete
{
    public class CoreCache : ICoreCache
    {
        public Dictionary<int, string> ResponseCodes { get; set; }
       

        public string GetResponseMsg(int responseCode)
        {
            string responsMsg = String.Empty;
            if (ResponseCodes!=null && ResponseCodes.ContainsKey(responseCode))
            {
                responsMsg = ResponseCodes[responseCode];
            }
            return responsMsg;
        }
    }
}
