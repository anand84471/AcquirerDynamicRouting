using ExpressCheckoutContracts.DTO;
using System.Threading.Tasks;

namespace ExpressCheckoutDb.Repository.Abstract
{
    public interface IMerchantRepo
    {
        Task<MerchantDto> GetMerchantData(int MerchantId);
        Task<bool> IsDuplicateMerchantIDAndMerchantOrderId(int merchantId, string merchantOrderID);
        Task<bool> ValidatePaymentMode(int merchantId, int PaymentMode);
        Task<int[]> GetPaymentModesMappedWithMerchant(int merchantId);
        Task<MerchantGatewayConfigurationMappingDto> GetMerchantPaymentGatewayConfigurationDetails(int merchantId, int gatewayId);
    }
}