using System.Threading.Tasks;
using Core.Cache.Abstract;
using Core.Constants;
using Core.Infrastructure.MVC;
using ExpressCheckout.BusinessLayer.Abstract;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Abstract;
using ExpressCheckoutContracts.Response.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExpressCheckout.Controllers
{
    [ApiController]
    [Route("api/customer/card")]
    public class SavedCardController : BaseApiController
    {
        private readonly ICustomerSavedCardValidation _CustomerSavedCardValidation;
        private readonly ICoreCache _CoreCache;
        private readonly ILogger<SavedCardController> _logger;

        public SavedCardController(ICustomerSavedCardValidation customerSavedCardValidation,
            ICoreCache CoreCache, ILogger<SavedCardController> _logger)
        {
            _CustomerSavedCardValidation = customerSavedCardValidation;
            _CoreCache = CoreCache;
            this._logger = _logger;
        }

        [HttpPost]
        [Route("save")]
        public async Task<MasterResponse> InsertCustomerCard([FromBody] SavedCardRequest savedCardRequest)
        {

            this._logger.LogInformation("Entered  InsertCustomerCard");
            CardRequest response = await _CustomerSavedCardValidation.InsertCustomerSavedCardValidation(savedCardRequest);
            response.Code = ResponseCodeConstants.SUCCESS;
            response.Message = _CoreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS);
            return response;

        }

        [HttpPost]
        [Route("fetch")]
        public async Task<MasterResponse> GetAllSaveCards(SavedCardRequest savedCardRequest)
        {

            this._logger.LogInformation("Entered  GetAllSaveCards");
            SaveCardResponse response = null;
            CardRequest[] cardRequestResponse = await _CustomerSavedCardValidation.GetAllSaveCard(savedCardRequest);

            return response = new SaveCardResponse
            {
                Code = ResponseCodeConstants.SUCCESS,
                Message = _CoreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS),
                cardDetailsResponse = cardRequestResponse
            };
        }

        [HttpPost]
        [Route("delete")]
        public async Task<MasterResponse> DeleteSaveCard(SavedCardRequest savedCardRequest)
        {

            this._logger.LogInformation("Entered  DeleteSaveCard");
            MasterResponse response = null;
            await _CustomerSavedCardValidation.DeleteSaveCard(savedCardRequest);
            return response = new MasterResponse
            {
                Code = ResponseCodeConstants.SUCCESS,
                Message = _CoreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS),
            };
        }


        [HttpPost]
        [Route("update/status")]
        public async Task<MasterResponse> UpdateStatusSaveCard(SavedCardRequest savedCardRequest)
        {

            this._logger.LogInformation("Entered  UpdateStatusSaveCard");
            MasterResponse response = null;
            await _CustomerSavedCardValidation.UpdateSaveCardStatus(savedCardRequest);
            return response = new MasterResponse
            {
                Code = ResponseCodeConstants.SUCCESS,
                Message = _CoreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS),
            };
        }
    }
}