using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutModule.ApiClients.Abstract;
using ExpressCheckoutModule.ServiceLayer.Abstract;

namespace ExpressCheckoutModule.ServiceLayer.Concrete
{
    class AndroidSDKService : IAndroidSDKService
    {
        private IAndroidPGSdkDBRepo _androidSDKDBRepo;
        private IPinePGApiClient _PinePGApiClient;

        public AndroidSDKService(IAndroidPGSdkDBRepo androidPGSdkDBRepo, IPinePGApiClient PinePGApiClient)
        {
            _androidSDKDBRepo = androidPGSdkDBRepo;
            _PinePGApiClient = PinePGApiClient;
        }

        public async Task<BankOTPUrlDTO[]>  GetBankUrlDetails(AndroidSdkGetUrlDetailsRequest objAndroidSdkGetUrlDetailsRequest)
        {
            return await _androidSDKDBRepo.GetOtpUrlDetails(objAndroidSdkGetUrlDetailsRequest);
        }

        public  async Task<bool> InsertAndroidSdkBrowserSessionDetails(AndroidPGSdkSessionDetailDTO objAdnroidPGSdkSessionDetailsModal)
        {
            
            bool status = await _androidSDKDBRepo.InsertBrowserSessionDetails(objAdnroidPGSdkSessionDetailsModal);
            if (!status)
            {
                throw new InvalidRequestException(ResponseCodeConstants.ANDROID_BROWSER_DETAIL_INSERTION_FAIL);
            }
            return status;
        }

        public async Task<bool> InsertJsExecutionError(MobileReportJsRequest mobileReportJsRequest)
        {
            return await _androidSDKDBRepo.InsertJsExecutionError(mobileReportJsRequest);
        }
        public async Task<bool> ReportTransactionStatus(ReportTransactionStatusDto reportTransactionStatusDto)
        {
            return await _androidSDKDBRepo.ReportTransactionStatus(reportTransactionStatusDto);
        }
    }
}
