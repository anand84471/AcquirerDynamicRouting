using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Cache.Abstract;
using Core.Constants;
using ExpressCheckout.BusinessLayer.Abstract;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Abstract;
using ExpressCheckoutContracts.Response.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpressCheckout.Controllers
{
    [Route("api/mobile")]
    [ApiController]
    public class MobileAppAPIController : ControllerBase
    {


        private readonly ICoreCache _coreCache;
        private readonly IMobileAppValidation _mobileAppValidation;

        /// <summary>
        /// Customer Api Constructor
        /// </summary>
        /// <param name="expressCheckoutCustomerValidation">Gate business layer for validations</param>
        public MobileAppAPIController(IMobileAppValidation mobileAppValidation, ICoreCache coreCache)
        {
            _coreCache = coreCache;
            _mobileAppValidation = mobileAppValidation;
        }



        [HttpPost]
        [Route("bank/urls")]
 
        public async Task<MasterResponse> GetBankUrlOtp([FromBody] AndroidSdkGetUrlDetailsRequest objAndroidSdkGetUrlDetailsRequest)
        {

            BankUrlOtpResponseDTO objResponse = null;
            BankURLDetailResponse[] bankURLDetailResponses = await _mobileAppValidation.GetOtpUrlDetails(objAndroidSdkGetUrlDetailsRequest);

            return objResponse = new BankUrlOtpResponseDTO
            {
                Code = ResponseCodeConstants.SUCCESS,
                Message = _coreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS),
                m_lsUrlDetailsModel = bankURLDetailResponses
            };
        }

        [HttpPost]
        [Route("bank/urls/ReportJsError")]
        public async Task<MasterResponse> ReportJsExecutionError([FromBody] MobileReportJsRequest mobileReportJsRequest)
        {
            MasterResponse response = null;
            bool status = await _mobileAppValidation.InsertJsExecutionError(mobileReportJsRequest);
            return response = new MasterResponse
            {
                Code = status ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE,
                Message = _coreCache.GetResponseMsg(status ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE),
            };
        }

        [HttpPost]
        [Route("bank/urls/InsertSdkBrowserDetails")]
        public async Task<MasterResponse> InsertBrowserSessionDetails([FromBody] AdnroidPGSdkSessionDetailsRequest objAdnroidPGSdkSessionDetailsRequest)
        {
            MasterResponse response = null;
            bool status = await _mobileAppValidation.InsertAndroidSdkBrowserSessionDetails(objAdnroidPGSdkSessionDetailsRequest);
            return response = new MasterResponse
            {
                Code = status ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE,
                Message = _coreCache.GetResponseMsg(status ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE),
            };           
        }

        [HttpPost]
        [Route("bank/urls/ReportTransactionStatus")]
        public async Task<MasterResponse> ReportTransactionStatus([FromBody] ReportTransactionStatusRequest objReportTransactionStatusRequest)
        {
            MasterResponse response = null;
            bool status = await _mobileAppValidation.ReportTransactionStatus(objReportTransactionStatusRequest);
            return response = new MasterResponse
            {
                Code = status ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE,
                Message = _coreCache.GetResponseMsg(status ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE),
            };
        }

    }
}