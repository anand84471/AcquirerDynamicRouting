using ExpressCheckoutContracts.DTO;
using System.Threading.Tasks;

namespace ExpressCheckoutDb.Repository.Abstract
{
    public interface ICustomerRepo
    {

        Task CreateCustomer(CustomerDto customerDto);

        Task<CustomerDto> GetCustomer(long customerId);
        Task UpdateCustomer(CustomerDto customerDto);
        Task<bool> IsDuplicateCutomerIdandRefernceNumber(int merchantId, string customerReferernceNo);
    }
}