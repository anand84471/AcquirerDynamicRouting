using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ServiceLayer.Abstract
{
    public interface IMerchantService
    {
        Task<MerchantDto> GetMerchantData(int merchantId);

        Task<bool> IsSavedCardEnabled(int merchantId);

        Task CheckIfMerchantExistsForMerchantId(int merchantId);
        bool IsMerchantValid(MerchantDto merchantDto, string merchantAccessCode);

        Task<bool> IsDuplicateMerchantIDAndMerchantOrderId(int merchantId, string merchantOrderID);

        Task<EnumPaymentMode[]> GetPaymentModesMappedWithMerchant(int merchantId);

        Task<MerchantGatewayConfigurationMappingDto> GetMerchantPaymentGatewayConfigurationDetails(int merchantId, int gatewayId);
    }
}