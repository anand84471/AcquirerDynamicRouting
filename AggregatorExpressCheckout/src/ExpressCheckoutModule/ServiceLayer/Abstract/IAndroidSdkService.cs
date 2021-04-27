using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;

namespace ExpressCheckoutModule.ServiceLayer.Abstract
{
    public interface IAndroidSDKService
    {
        Task<bool> InsertJsExecutionError(MobileReportJsRequest mobileReportJsRequest);
        Task<BankOTPUrlDTO[]> GetBankUrlDetails(AndroidSdkGetUrlDetailsRequest objAndroidSdkGetUrlDetailsRequest);

        Task<bool> InsertAndroidSdkBrowserSessionDetails(AndroidPGSdkSessionDetailDTO objAdnroidPGSdkSessionDetailsModal);
        Task<bool> ReportTransactionStatus(ReportTransactionStatusDto reportTransactionStatusDto);

    }
}
