using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TorrentPayController.BusinessLayer.Abstract
{
    public interface ITorrentPayValidation
    {
        Task<BankURLDetailResponse[]> GetOtpUrlDetails(AndroidSdkGetUrlDetailsRequest objAndroidSdkGetUrlDetailsRequest);
        Task<bool> InsertJsExecutionError(MobileReportJsRequest mobileReportJsRequest);
        Task<bool> InsertTorrentayTxnDetails(TorrentPayTransactionDetailsRequest torrentPayTransactionDetailsRequest);
        Task<bool> InsertAndroidSdkBrowserSessionDetails(AdnroidPGSdkSessionDetailsRequest OBJobjAndroidSdkGetUrlDetailsRequest);
        Task<bool> ReportTransactionStatus(ReportTransactionStatusRequest objReportTransactionStatusRequest);
    }
}
