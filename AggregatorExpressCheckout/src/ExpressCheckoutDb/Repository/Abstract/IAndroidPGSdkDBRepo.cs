using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;

namespace ExpressCheckoutDb.Repository.Abstract
{
    public interface IAndroidPGSdkDBRepo
    {
        Task<bool> InsertJsExecutionError(MobileReportJsRequest mobileReportJsRequest);
        Task<BankOTPUrlDTO[]> GetOtpUrlDetails(AndroidSdkGetUrlDetailsRequest objAndroidSdkGetUrlDetailsRequest);
        Task<bool> InsertBrowserSessionDetails(AndroidPGSdkSessionDetailDTO objAdnroidPGSdkSessionDetailDTO);
        Task<bool> ReportTransactionStatus(ReportTransactionStatusDto reportTransactionStatusDto);
    }
}
