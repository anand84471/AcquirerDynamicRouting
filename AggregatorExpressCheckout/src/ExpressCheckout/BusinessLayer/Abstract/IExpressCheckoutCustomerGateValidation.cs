using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Concrete;
using System.Threading.Tasks;

namespace ExpressCheckout.BusinessLayer.Abstract
{
    /// <summary>
    /// Gate business layer for customer validations
    /// </summary>
    public interface IExpressCheckoutCustomerGateValidation
    {
        Task<CustomerResponse> CreateCustomer(CreateCustomerRequest customerRequest);

        Task<CustomerResponse> GetCustomer(int merchantId,long customerId);
       

        Task<CustomerResponse> UpdateCustomer(long customerId, UpdateCustomerRequest customerRequest);
    }
}
