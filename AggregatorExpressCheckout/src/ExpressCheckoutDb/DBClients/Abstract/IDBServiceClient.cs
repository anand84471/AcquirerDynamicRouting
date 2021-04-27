using AggExpressCheckoutDBService;
using System;

namespace ExpressCheckoutDb.DBClients.Abstarct
{
    public interface IDBServiceClient : IDisposable
    {
        AggregatorExpressCheckoutServiceClient _AggregatorExpressCheckoutServiceClient { get; set; }

    }

}