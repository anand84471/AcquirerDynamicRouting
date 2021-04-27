using Core.Constants;
using DynamicRouting.BusinessLayer.Abstract;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicRouting.BusinessLayer.Concrete
{
    public class DefaultRoutingGatewaySortingStartegy : IRoutingGatewaySortingStartegy
    {
        public DefaultRoutingGatewaySortingStartegy()
        {

        }
        public List<EnumGateway> Sort(List<RoutingWiseDetailsDto> simpleRoutingWiseDetailsDto, List<RoutingWiseDetailsDto> customizeRoutingWiseDetailsDto, List<RoutingWiseDetailsDto> specialRoutingWiseDetailsDto)
        {

            List<EnumGateway> lsFinalGateways = new List<EnumGateway>();
            List<RoutingWiseDetailsDto> lsGatwaysPriorityMoreThanCustomize = new List<RoutingWiseDetailsDto>();
            List<RoutingWiseDetailsDto> lsGatwaysPriorityLessThanCustomize = new List<RoutingWiseDetailsDto>();



            specialRoutingWiseDetailsDto.ForEach(x =>
            {
                if (x.prefernceScore > RoutingConstants.PREFERNCE_SCORE_OF_CUSTOMIZE_AND_SIMPLE_ROUTING)
                {
                    lsGatwaysPriorityMoreThanCustomize.Add(x);

                }
                else
                {
                    lsGatwaysPriorityLessThanCustomize.Add(x);
                }

            });

            lsFinalGateways.AddRange(lsGatwaysPriorityMoreThanCustomize.OrderByDescending(x => x.prefernceScore)
                .SelectMany(x => x.enumGatewaysList).ToList());
            lsFinalGateways.AddRange(customizeRoutingWiseDetailsDto.SelectMany(x => x.enumGatewaysList).ToList());
            lsFinalGateways.AddRange(simpleRoutingWiseDetailsDto.SelectMany(x => x.enumGatewaysList).ToList());
            lsFinalGateways.AddRange(lsGatwaysPriorityLessThanCustomize.OrderByDescending(x => x.prefernceScore)
               .SelectMany(x => x.enumGatewaysList).ToList());

            return lsFinalGateways;
        }
    }
}
