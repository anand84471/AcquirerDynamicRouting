using Core.Cache.Abstract;
using Core.Constants;
using ExpressCheckout.BusinessLayer.Abstract;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Abstract;
using ExpressCheckoutContracts.Response.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExpressCheckout.Controllers
{
    [Route("api/merchant")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private readonly ICoreCache _coreCache;

        private readonly IMerchantValidation _merchantValidation;

        private readonly ILogger<MerchantController> _logger;

        /// <summary>
        /// Customer Api Constructor
        /// </summary>
        /// <param name="expressCheckoutCustomerValidation">Gate business layer for validations</param>
        public MerchantController(IMerchantValidation merchantValidation, ICoreCache coreCache , ILogger<MerchantController> _logger)
        {
            _coreCache = coreCache;
            _merchantValidation = merchantValidation;
            this._logger = _logger;
        }

        [Route("paymentmodes")]
        public async Task<MasterResponse> GetPaymentModesMappedWithMerchant(OrderDetailsRequest orderDetailsRequest)
        {
            this._logger.LogInformation("Entered  GetPaymentModesMappedWithMerchant");
            MerchantResponse response = null;
            EnumPaymentMode[] enumPaymentModes = await _merchantValidation.GetPaymentMode(orderDetailsRequest);
            return response = new MerchantResponse
            {
                Code = ResponseCodeConstants.SUCCESS,
                Message = _coreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS),
                enumPaymentModes = enumPaymentModes
            };

        }

    }
}
