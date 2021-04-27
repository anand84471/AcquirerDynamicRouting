using ExpressCheckoutContracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO
{
    public class RoutingWiseDetailsDto
    {
        public List<EnumGateway>enumGatewaysList{get;set;}

        public int prefernceScore { get; set; }


    }
}
