using ExpressCheckoutContracts.Enums;
using System.Threading.Tasks;

namespace ExpressCheckoutDb.Repository.Abstract
{
    public interface INetbankingRepo
    {
        Task<string> GetNetbankingPaymentOptionCode(EnumGateway enumGateway, string paymentCode);
    }
}
