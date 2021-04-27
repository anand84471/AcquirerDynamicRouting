using ExpressCheckoutContracts;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpressCheckoutDb.Repository.Abstract
{
    public interface IDynamicRoutingRepo
    {
        Task<DynamicRoutingDetailsDto> GetDynamicRoutingDetails(int MerchantId);
        Task<SpecialRoutingDetailsDto[]> GetGatewayAccToCardBinRoutingConfig(long configId, string cardBin);
        Task<SpecialRoutingDetailsDto[]> GetGatewayAccToCardBrandRoutingConfig(long configId, int assosiactionTypeId);
        Task<SpecialRoutingDetailsDto[]> GetGatewayAccToIssuerRoutingConfig(long configId, int issuerId);
        Task<MerchantRoutingConfigDetailsDto> GetRoutingCOnfigurationDetails(int MerchantId);
    }
}
