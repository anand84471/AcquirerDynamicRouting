using System;
using System.Threading.Tasks;
using Core.Constants;
using ExpressCheckout.BusinessLayer.Abstract;
using ExpressCheckoutContracts.Response.Abstract;
using ExpressCheckoutContracts.Response.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace ExpressCheckout.Controllers
{
   
    [ApiController]
    public class PayUController : ControllerBase
    {
        private readonly IPayUValidation payUValidation;
        public PayUController(IPayUValidation payUValidation)
        {
            this.payUValidation = payUValidation;
        }
        [HttpPost]
        [Route("payu/responseHandler")]
        public async Task<MasterResponse> PayuResponseHandler()
        {
            try
            {
                var collection = HttpContext.Request.Form;
                if (collection == null)
                {
                    return null;
                }
                var data = await this.payUValidation.CompletePayUTransaction(collection);
                return new PayUPurchaseResponse
                {
                    Code = ResponseCodeConstants.SUCCESS,
                    Message = "SUCCESS",
                    OrderDetails = data
                };
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}