using ExpressCheckoutContracts;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Requests.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicRouting.BusinessLayer.Abstract
{
    public interface IRoutingEngineService
    {
        Task<List<string>> GetGateways(RoutingRequest routingRequest);
    }
}
