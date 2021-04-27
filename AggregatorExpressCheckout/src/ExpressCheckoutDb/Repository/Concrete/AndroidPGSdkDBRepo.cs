using AggExpressCheckoutDBService;
using AutoMapper;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutDb.DBClients.Abstarct;
using ExpressCheckoutDb.Repository.Abstract;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Enums;
using System.Text;
using Microsoft.Extensions.Logging;
using Core.Features.ExceptionHandling.Concrete;
using Core.Constants;

namespace ExpressCheckoutDb.Repository.Concrete
{
    public class AndroidPGSdkDBRepo : IAndroidPGSdkDBRepo
    {
        private readonly IMapper mapper;

        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<AndroidPGSdkDBRepo> _logger;

        /// <summary>Initializes a new instance of the <see cref="CustomerRepo"/> class.</summary>
        /// <param name="aggregatorExpressCheckoutServiceClient">The aggregator express checkout service client.</param>
        public AndroidPGSdkDBRepo(IMapper mapper, IServiceProvider serviceProvider, ILogger<AndroidPGSdkDBRepo> _logger)
        {
            this.mapper = mapper;
            this._serviceProvider = serviceProvider;
            this._logger = _logger;
        }

        public async Task<BankOTPUrlDTO[]> GetOtpUrlDetails(AndroidSdkGetUrlDetailsRequest objAndroidSdkGetUrlDetailsRequest)
        {
            BankOTPUrlDTO[] arrBankOtpUrlDTO = null;
            BankOTPUrlEntity[] arrBankOtpUrlEntity;
            try
            {
                using (IDBServiceClient serviceClient = this._serviceProvider.GetService<IDBServiceClient>())
                {
                    arrBankOtpUrlEntity = await serviceClient._AggregatorExpressCheckoutServiceClient.GetBankOtpUrlsAsync(objAndroidSdkGetUrlDetailsRequest.iClientId, objAndroidSdkGetUrlDetailsRequest.bIsRequiredNetBankingDetails);
                }
                if (arrBankOtpUrlEntity == null || arrBankOtpUrlEntity.Length == 0)
                {
                    throw new InvalidRequestException(ResponseCodeConstants.DB_FAILURE);
                }
                arrBankOtpUrlDTO = mapper.Map<BankOTPUrlDTO[]>(arrBankOtpUrlEntity);
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return arrBankOtpUrlDTO;
        }

        public  async Task<bool> InsertBrowserSessionDetails(AndroidPGSdkSessionDetailDTO objAdnroidPGSdkSessionDetailDTO)
        {
            AndroidSdkSessionDetailsEntity androidSdkSessionDetailsEntity = mapper.Map<AndroidSdkSessionDetailsEntity>(objAdnroidPGSdkSessionDetailDTO);
            bool result = false;
            try
            {
                using (IDBServiceClient serviceClient = this._serviceProvider.GetService<IDBServiceClient>())
                {
                    result = await serviceClient._AggregatorExpressCheckoutServiceClient.InsertAndroidSdkBrowserSessionDetailsAsync(androidSdkSessionDetailsEntity);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return result;
        }
        

        public  async Task<bool> InsertJsExecutionError(MobileReportJsRequest mobileReportJsRequest)
       {
            bool result = false;
            try
            {
                //EdgeXCeedJsExceptionDetailsEntity objEdgeXCeedJsExceptionDetailsEntity = mapper.Map<EdgeXCeedJsExceptionDetailsEntity>(mobileReportJsRequest);
                EdgeXCeedJsExceptionDetailsEntity objEdgeXCeedJsExceptionDetailsEntity = new EdgeXCeedJsExceptionDetailsEntity();
                objEdgeXCeedJsExceptionDetailsEntity.m_iClientTypeId = mobileReportJsRequest.m_iClientTypeId;
                objEdgeXCeedJsExceptionDetailsEntity.m_strClientVersion = mobileReportJsRequest.m_strClientVersion;
                objEdgeXCeedJsExceptionDetailsEntity.m_iExecutedJsCodeTypeId = mobileReportJsRequest.m_iExecutedJsCodeTypeId;
                objEdgeXCeedJsExceptionDetailsEntity.m_strExceptionUrl = mobileReportJsRequest.m_strExceptionUrl;
                objEdgeXCeedJsExceptionDetailsEntity.m_strExecutedJsCode = mobileReportJsRequest.m_strExecutedJsCode;
                objEdgeXCeedJsExceptionDetailsEntity.m_strExceptionMessage = mobileReportJsRequest.m_strExceptionMessage;
                objEdgeXCeedJsExceptionDetailsEntity.m_strHtmlOfExceptionUrl = mobileReportJsRequest.m_strHtmlOfExceptionUrl;
                using (IDBServiceClient serviceClient = this._serviceProvider.GetService<IDBServiceClient>())
                {
                    result = await serviceClient._AggregatorExpressCheckoutServiceClient.InsertEdgeXceedJsExceptonAsync(objEdgeXCeedJsExceptionDetailsEntity);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());

            }
            return result;
        }


        public async Task<bool> ReportTransactionStatus(ReportTransactionStatusDto reportTransactionStatusDto)
        {
            bool result = false;
            ReportTransactionStatusEntity reportTransactionStatusEntity = mapper.Map<ReportTransactionStatusEntity>(reportTransactionStatusDto);
            try
            {
                using (IDBServiceClient serviceClient = this._serviceProvider.GetService<IDBServiceClient>())
                {
                    bool IsTransactionIdExistsForMerchant = false;
                    IsTransactionIdExistsForMerchant = await serviceClient._AggregatorExpressCheckoutServiceClient.CheckIsTransactionIdExistsForMerchantForAndroidSdkAsync(reportTransactionStatusEntity);
                    if (IsTransactionIdExistsForMerchant)
                    {
                        result = await serviceClient._AggregatorExpressCheckoutServiceClient.ChangeAndroidSdkTransactionStatusAsync(reportTransactionStatusEntity);
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());

            }
            return result;
        }
    }
}
