using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract
{
    public interface ICommonIntegrationHandlerService
    {
        //Task<List<EnumGateway>> GetGatewaysAccordingToDynamicRouting(int merchantId);

        Task<string> UpdateTransactionResponseAndCreateResponseForMerchant(FinalResponseHelperDto finalResponseHelperDto);
    }
}
