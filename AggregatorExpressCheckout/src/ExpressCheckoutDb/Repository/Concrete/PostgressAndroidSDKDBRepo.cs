using AutoMapper;
using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutDb.DBClients.Abstarct;
using ExpressCheckoutDb.DBClients.Abstract;
using ExpressCheckoutDb.DBClients.Concrete;
using ExpressCheckoutDb.Postgress.Abstract;
using ExpressCheckoutDb.Repository.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ExpressCheckoutDb.Entities;
using ExpressCheckoutDb.Entities.Concrete;

namespace ExpressCheckoutDb.Repository.Concrete
{
    class PostgressAndroidSDKDBRepo : IAndroidPGSdkDBRepo
    {
        private readonly IMapper mapper;

        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<PostgressAndroidSDKDBRepo> _logger;

        public PostgressAndroidSDKDBRepo(IMapper mapper, IServiceProvider serviceProvider, ILogger<PostgressAndroidSDKDBRepo> _logger)
        {
            this.mapper = mapper;
            this._serviceProvider = serviceProvider;
            this._logger = _logger;
        }
        public async Task<BankOTPUrlDTO[]> GetOtpUrlDetails(AndroidSdkGetUrlDetailsRequest objAndroidSdkGetUrlDetailsRequest)
        {
            BankOTPUrlDTO[] arrBankOtpUrlDTO = null;
            TorrentPayBankOTPUrlDetailEntity[] arrBankOtpUrlEntity;
            try
            {
                using (IPostgressDBServiceClient serviceClient = this._serviceProvider.GetService<IPostgressDBServiceClient>())
                {
                    arrBankOtpUrlEntity = await serviceClient._EdgeXCeedDBService.GetBankOtpUrls(objAndroidSdkGetUrlDetailsRequest.iClientId, objAndroidSdkGetUrlDetailsRequest.bIsRequiredNetBankingDetails);
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

        public async Task<bool> InsertBrowserSessionDetails(AndroidPGSdkSessionDetailDTO objAdnroidPGSdkSessionDetailDTO)
        {
            TorrentPaySessionDetailsEntity androidSdkSessionDetailsEntity = mapper.Map<TorrentPaySessionDetailsEntity>(objAdnroidPGSdkSessionDetailDTO);
            bool result = false;
            try
            {
                using (IPostgressDBServiceClient serviceClient = this._serviceProvider.GetService<IPostgressDBServiceClient>())
                {
                    result = await serviceClient._EdgeXCeedDBService.InsertEdgeBroserSessionDetails(androidSdkSessionDetailsEntity);
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

        public async Task<bool> InsertJsExecutionError(MobileReportJsRequest mobileReportJsRequest)
        {

            bool result = false;
            try
            {
                TorrentPayJsExceptionEntity objEdgeXCeedJsExceptionDetailsEntity = new TorrentPayJsExceptionEntity();
                objEdgeXCeedJsExceptionDetailsEntity = mapper.Map<TorrentPayJsExceptionEntity>(mobileReportJsRequest);
                using (IPostgressDBServiceClient serviceClient = this._serviceProvider.GetService<IPostgressDBServiceClient>())
                {
                    result = await serviceClient._EdgeXCeedDBService.InertNewJsExceptionDetails(objEdgeXCeedJsExceptionDetailsEntity);
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
            try
            {
                TorrentPayChangeTxnStatusEntity objEdgeXCeedJsExceptionDetailsEntity = new TorrentPayChangeTxnStatusEntity();
                objEdgeXCeedJsExceptionDetailsEntity = mapper.Map<TorrentPayChangeTxnStatusEntity>(reportTransactionStatusDto);
                using (IPostgressDBServiceClient serviceClient = this._serviceProvider.GetService<IPostgressDBServiceClient>())
                {
                    result = await serviceClient._EdgeXCeedDBService.ChangeTorrentPayTxnStatus(objEdgeXCeedJsExceptionDetailsEntity);
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
