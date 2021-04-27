using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using System.Threading.Tasks;

namespace ExpressCheckoutDb.Repository.Abstract
{
    public interface IGatewayRepo
    {
        Task<GatewayDto> GetGatewayDetails(EnumGateway enumGateway);
        Task<EnableGatewayPaymentModeDto> GetEnableGatewayList(int merchantId, int paymentId);
    }
}
