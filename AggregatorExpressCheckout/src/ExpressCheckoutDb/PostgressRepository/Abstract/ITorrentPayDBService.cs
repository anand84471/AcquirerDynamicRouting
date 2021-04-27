using ExpressCheckoutDb.Entities;
using ExpressCheckoutDb.Entities.Concrete;
using System;
using System.Threading.Tasks;

namespace ExpressCheckoutDb.Postgress.Abstract
{
    public interface ITorrentPayDBService : IDisposable
    {
        Task<TorrentPayBankOTPUrlDetailEntity[]> GetBankOtpUrls(int cliendId, bool isNetbankingDetailsRequired);
        Task<bool> InsertEdgeBroserSessionDetails(TorrentPaySessionDetailsEntity objAndroidSdkSessionDetailsEntity);
        Task<bool> InertNewJsExceptionDetails(TorrentPayJsExceptionEntity edgeXceedJsExceptionEntity);
        Task<bool> ChangeTorrentPayTxnStatus(TorrentPayChangeTxnStatusEntity changeTxnStatusEntity);
    }
}
