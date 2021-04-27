using System;
using System.Threading.Tasks;
using Core.Constants;
using ExpressCheckout.BusinessLayer.Abstract;
using ExpressCheckoutContracts.Response.Abstract;
using ExpressCheckoutContracts.Response.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpressCheckout.Controllers
{
    [ApiController]
    public class RazorpayController : ControllerBase
    {
        private readonly IRazorpayValidation razorpayValidation;
        public RazorpayController(IRazorpayValidation razorpayValidation)
        {
            this.razorpayValidation = razorpayValidation;
        }

        [HttpPost]
        [Route("razorpay/responseHandler")]
        public async Task<MasterResponse> RazorpayResponseHandler()
        {
            try
            {
                var collection = HttpContext.Request.Form;
                if (collection == null)
                {
                    return null;
                }
                var data = await this.razorpayValidation.CompleteRazorpayTransaction(collection);
                return new RazorpayPurchaseResponse
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