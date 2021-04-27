using Core.Constants;
using Core.Infrastructure.MVC;
using ExpressCheckout.BusinessLayer.Abstract;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Abstract;
using ExpressCheckoutModule.Cache.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExpressCheckout.Controllers
{
    /// <summary>
    /// Customer Api
    /// </summary>
    public class CustomerController : BaseApiController
    {
        private readonly IExpressCheckoutCache _CoreCache;

        private readonly IExpressCheckoutCustomerGateValidation expressCheckoutCustomerValidation;

        private readonly ILogger<CustomerController> _logger;


        /// <summary>
        /// Customer Api Constructor
        /// </summary>
        /// <param name="expressCheckoutCustomerValidation">Gate business layer for validations</param>
        public CustomerController(IExpressCheckoutCustomerGateValidation expressCheckoutCustomerValidation, 
            IExpressCheckoutCache _CoreCache, ILogger<CustomerController> _logger)
        {
            this._CoreCache = _CoreCache;
            this.expressCheckoutCustomerValidation = expressCheckoutCustomerValidation;
            this._logger = _logger;
        }

        /// <summary>
        /// Gets the customer
        /// </summary>
        [HttpPost]
        [Route("create/customer")]
        public async Task<MasterResponse> CreateCustomer([FromBody]CreateCustomerRequest customerRequest)
        {
            this._logger.LogInformation("Entered  CreateCustomer");            
            var response = await this.expressCheckoutCustomerValidation.CreateCustomer(customerRequest);
            response.Code = ResponseCodeConstants.SUCCESS;
            response.Message = _CoreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS);
            return response;
        }

        [HttpGet]
        [Route("customer/{merchantId}/{customerId}")]
        public async Task<MasterResponse> GetCustomer(long customerId, int merchantId)
        {
            this._logger.LogInformation("Entered  GetCustomer CustomerId {0}",customerId);
            var response = await this.expressCheckoutCustomerValidation.GetCustomer(merchantId, customerId);
            response.Code = ResponseCodeConstants.SUCCESS;
            response.Message = _CoreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS);
            return response;
        }


        [HttpPost]
        [Route("update/customer/{customerId}")]
        public async Task<MasterResponse> UpdateCustomer(long customerId, [FromBody] UpdateCustomerRequest customerRequest)
        {
            this._logger.LogInformation("Entered  UpdateCustomer customerId {0}",customerId);
            var response = await this.expressCheckoutCustomerValidation.UpdateCustomer(customerId, customerRequest);
            response.Code = ResponseCodeConstants.SUCCESS;
            response.Message = _CoreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS);
            return response;
        }
    }
}