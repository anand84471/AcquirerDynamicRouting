using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Concrete;

namespace ExpressCheckout.BusinessLayer.Abstract
{
    public interface IMobileAppValidation
    {
        Task<BankURLDetailResponse[]> GetOtpUrlDetails(AndroidSdkGetUrlDetailsRequest objAndroidSdkGetUrlDetailsRequest);
        Task<bool> InsertJsExecutionError(MobileReportJsRequest mobileReportJsRequest);
        Task<bool> InsertAndroidSdkBrowserSessionDetails(AdnroidPGSdkSessionDetailsRequest OBJobjAndroidSdkGetUrlDetailsRequest);
        Task<bool> ReportTransactionStatus(ReportTransactionStatusRequest objReportTransactionStatusRequest);
    }
}
