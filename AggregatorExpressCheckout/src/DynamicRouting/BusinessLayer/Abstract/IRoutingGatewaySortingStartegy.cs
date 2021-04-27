using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicRouting.BusinessLayer.Abstract
{
    public interface IRoutingGatewaySortingStartegy
    {
        public List<EnumGateway> Sort(List<RoutingWiseDetailsDto> simpleRoutingWiseDetailsDto, List<RoutingWiseDetailsDto> customizeRoutingWiseDetailsDto,
            List<RoutingWiseDetailsDto> specialRoutingWiseDetailsDto);

    }
}
