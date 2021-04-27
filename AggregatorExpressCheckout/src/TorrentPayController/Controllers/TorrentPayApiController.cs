using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Cache.Abstract;
using Core.Constants;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Abstract;
using ExpressCheckoutContracts.Response.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TorrentPayController.BusinessLayer.Abstract;
using static System.Net.Mime.MediaTypeNames;

namespace TorrentPayController.Controllers
{
    [Route("api/mobile")]
    [ApiController]
    public class TorrentPayApiController : ControllerBase
    {
        private readonly ICoreCache _coreCache;
        private readonly ITorrentPayValidation _torrentPayValidation;
        private readonly ILogger<TorrentPayApiController> _logger;

        /// <summary>
        /// Customer Api Constructor
        /// </summary>
        /// <param name="expressCheckoutCustomerValidation">Gate business layer for validations</param>
        public TorrentPayApiController(ITorrentPayValidation mobileAppValidation, ICoreCache coreCache, ILogger<TorrentPayApiController> _logger)
        {
            _coreCache = coreCache;
            _torrentPayValidation = mobileAppValidation;
            this._logger = _logger;
        }

        [HttpGet]
        [Route("index")]

        public ActionResult Index()
        {
            return StatusCode(200);
        }
        [HttpGet]
        [Route("uat/index")]

        public ActionResult Uat()
        {
            return StatusCode(200);
        }
        [HttpPost]
        [Route("bank/urls")]

        public async Task<MasterResponse> GetBankUrlOtp([FromBody] AndroidSdkGetUrlDetailsRequest objAndroidSdkGetUrlDetailsRequest)
        {

            BankUrlOtpResponseDTO objResponse = null;
            try
            {
                BankURLDetailResponse[] bankURLDetailResponses = await _torrentPayValidation.GetOtpUrlDetails(objAndroidSdkGetUrlDetailsRequest);
                objResponse = new BankUrlOtpResponseDTO
                {
                    Code = ResponseCodeConstants.SUCCESS,
                    Message = _coreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS),
                    m_lsUrlDetailsModel = bankURLDetailResponses
                };
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return objResponse;
        }
        private byte[] Compress(byte[] raw)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory,
                    CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }
       
        [HttpPost]
        [Route("v2/bank/urls")]

        public async Task<V2BankUrlDetailsResponse> GetBankUrlDetailV2([FromBody] AndroidSdkGetUrlDetailsRequest objAndroidSdkGetUrlDetailsRequest)
        {
            V2BankUrlDetailsResponse objResponse=null;
            
            try
            {
                this._logger.LogInformation("{0} method called with client tye id {1} and is_rquied_netbanking_details", "GetBankUrlDetailV2s",
                    objAndroidSdkGetUrlDetailsRequest.iClientId, objAndroidSdkGetUrlDetailsRequest.bIsRequiredNetBankingDetails);
                BankURLDetailResponse[] bankURLDetailResponses = null;
                bankURLDetailResponses = await _torrentPayValidation.GetOtpUrlDetails(objAndroidSdkGetUrlDetailsRequest);
                string text = JsonConvert.SerializeObject(bankURLDetailResponses);
                if(text!=null)
                {
                    this._logger.LogInformation("Bank urls response data seralized successfully");
                }
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                objResponse = new V2BankUrlDetailsResponse
                {
                    Code = ResponseCodeConstants.SUCCESS,
                    Message = _coreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS),
                    urlData = Convert.ToBase64String(Compress(bytes))
                };
                if (objResponse.urlData != null)
                {
                    this._logger.LogInformation("Bank urls  data compressed successfully");
                }
            }
            catch(Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return objResponse;
        }
        
        [HttpPost]
        [Route("bank/urls/ReportJsError")]
        public async Task<MasterResponse> ReportJsExecutionError([FromBody] MobileReportJsRequest mobileReportJsRequest)
        {
            MasterResponse response = null;
            try
            {
                bool status = await _torrentPayValidation.InsertJsExecutionError(mobileReportJsRequest);
                response = new MasterResponse
                {
                    Code = status ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE,
                    Message = _coreCache.GetResponseMsg(status ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE),
                };
            }
            catch(Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return response;
        }
        [HttpPost]
        [Route("bank/urls/InsertTorrebtPayTxnDetails")]
        public async Task<MasterResponse> InsertTorrebtPayTxnDetails([FromBody] TorrentPayTransactionDetailsRequest torrentPayTransactionDetailsRequest)
        {
            MasterResponse response = null;
            try
            {
                bool status = await _torrentPayValidation.InsertTorrentayTxnDetails(torrentPayTransactionDetailsRequest);
                response = new MasterResponse
                {
                    Code = status ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE,
                    Message = _coreCache.GetResponseMsg(status ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE),
                };
            }
            catch(Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return response;
        }
        [HttpPost]
        [Route("bank/urls/InsertSdkBrowserDetails")]
        public async Task<MasterResponse> InsertBrowserSessionDetails([FromBody] AdnroidPGSdkSessionDetailsRequest objAdnroidPGSdkSessionDetailsRequest)
        {
            MasterResponse response = null;
            try
            {
                bool status = await _torrentPayValidation.InsertAndroidSdkBrowserSessionDetails(objAdnroidPGSdkSessionDetailsRequest);
                return response = new MasterResponse
                {
                    Code = status ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE,
                    Message = _coreCache.GetResponseMsg(status ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE),
                };
            }
            catch(Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return response;
        }

        [HttpPost]
        [Route("bank/urls/ReportTransactionStatus")]
        public async Task<MasterResponse> ReportTransactionStatus([FromBody] ReportTransactionStatusRequest objReportTransactionStatusRequest)
        {
            MasterResponse response = null;
            try
            {
                bool status = await _torrentPayValidation.ReportTransactionStatus(objReportTransactionStatusRequest);
                response = new MasterResponse
                {
                    Code = status ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE,
                    Message = _coreCache.GetResponseMsg(status ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE),
                };
            }
            catch(Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return response;
        }
    }
}