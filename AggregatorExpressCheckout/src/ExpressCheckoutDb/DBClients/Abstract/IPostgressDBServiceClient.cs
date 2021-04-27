using ExpressCheckoutDb.Postgress.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutDb.DBClients.Abstract
{
    public interface IPostgressDBServiceClient:IDisposable
    {
        TorrentPayDBService _EdgeXCeedDBService { get; set; }
    }
}
