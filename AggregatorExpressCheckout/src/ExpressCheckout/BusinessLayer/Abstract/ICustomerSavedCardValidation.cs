using ExpressCheckoutContracts.Requests;
using System.Threading.Tasks;

namespace ExpressCheckout.BusinessLayer.Abstract
{
    public interface ICustomerSavedCardValidation
    {
        Task<CardRequest> InsertCustomerSavedCardValidation(SavedCardRequest savedCardRequest);

        Task<CardRequest[]> GetAllSaveCard(SavedCardRequest savedCardRequest);

        Task<bool> DeleteSaveCard(SavedCardRequest savedCardRequest);

        Task<bool> UpdateSaveCardStatus(SavedCardRequest savedCardRequest);
    }
}