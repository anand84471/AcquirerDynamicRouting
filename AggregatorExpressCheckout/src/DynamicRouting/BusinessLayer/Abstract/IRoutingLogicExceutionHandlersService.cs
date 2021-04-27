using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.Routing;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicRouting.BusinessLayer.Abstract
{
    public interface IRoutingLogicExceutionHandlersService
    {

     Task<List<RoutingWiseDetailsDto>> ExecuteAndFetch(OrderDetailsDto orderDetailsDtoFromDB, RoutingRequest routingRequest,
          MerchantRoutingConfigDetailsDto merchantRoutingConfigDetailsDto);
    }
}
