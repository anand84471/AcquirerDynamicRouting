using ExpressCheckoutContracts.DTO;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ServiceLayer.Abstract
{
    public interface ICustomerSavedCardService
    {
        Task<bool> InsertSavedCardDetails(SavedCardDto savedCardDto);

        Task<bool> DeleteSavedCardDetails(long customerId, long savedCardId);

        Task<bool> UpdateSavedCardStatus(long customerId, long savedCardId, int status);

        Task<CardDetailsDto[]> GetAllSavedCards(long customerId);

        Task<bool> CheckIsSavedCardIdMappedWithCustomer(long customerId, long savedCardId);
    }
}