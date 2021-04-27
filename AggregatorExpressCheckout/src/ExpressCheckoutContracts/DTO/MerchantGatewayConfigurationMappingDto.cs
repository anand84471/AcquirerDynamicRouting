using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO
{
    public class MerchantGatewayConfigurationMappingDto
    {
     
        public int MerchantId { get; set; }

       
        public int GatewayId { get; set; }

       
        public short Status { get; set; }

        
        public string MerchantIdIssuedByGatewayToMerchant { get; set; }

       
        public string PasswordIssuedByGatewayToMerchant { get; set; }

       
        public string SecretKeyIssuedByGatewayToMerchant { get; set; }
    }
}
