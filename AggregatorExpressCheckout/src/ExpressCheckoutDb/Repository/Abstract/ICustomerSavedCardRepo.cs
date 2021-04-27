using ExpressCheckoutContracts.DTO;
using System.Threading.Tasks;

namespace ExpressCheckoutDb.Repository.Abstract
{
    public interface ICustomerSavedCardRepo //: IEntityRespositoryBase<CustomerSavedCardEntity>
    {
        Task<bool> Save(SavedCardDto savedCardDto);

        Task<bool> DeleteSavedCard(long customerId, long savedCardId);

        Task<bool> UpdateSavedCardStatus(long customerId, long savedCardId,int iStatus);

        Task<CardDetailsDto[]> GetAllSavedCard(long customerId);
    }
}