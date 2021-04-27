using System;
using System.Collections.Generic;

namespace Core.Cache.Abstract
{
    public interface ICoreCache
    {
        Dictionary<int, string> ResponseCodes { get; set; }


        String GetResponseMsg(int responseCode);
    }
}
