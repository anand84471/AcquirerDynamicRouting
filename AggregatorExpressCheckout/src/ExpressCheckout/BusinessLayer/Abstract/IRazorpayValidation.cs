using ExpressCheckoutContracts.DTO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ExpressCheckout.BusinessLayer.Abstract
{
    public interface IRazorpayValidation
    {
        Task<string> CompleteRazorpayTransaction(IFormCollection formCollection);
    }
}
