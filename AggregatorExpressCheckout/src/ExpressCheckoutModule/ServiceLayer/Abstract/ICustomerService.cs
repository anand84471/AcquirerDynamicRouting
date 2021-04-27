using ExpressCheckoutContracts.DTO;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ServiceLayer.Abstract
{
    public interface ICustomerService
    {
        Task CreateCustomer(CustomerDto customerDto);

        Task<bool> CheckIsCustomerIdMappedWithMerchant(int MerchantId, long CustomerId);

        Task<CustomerDto> GetCustomer(long customerId);

        Task UpdateCustomer(CustomerDto customerDto);

        Task<bool> IsDuplicateCutomerIdandRefernceNumber(int merchantId, string customerReferernceNo);
    }
}