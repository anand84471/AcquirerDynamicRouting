using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutDb.Entities
{
    public class TorrentPayBankOTPUrlDetailEntity
    {
      
        public string bankName { get; set; }
      
        public string url { get; set; }
      
        public bool isPageToBeResponsive { get; set; }
      
        public bool isOTPUrl { get; set; }
      
        public bool isPageNetBankingSubmissionPage { get; set; }
      
        public string strPageCustomizationJsCode { get; set; }
      
        public string strNetBankingSubmissionJsCode { get; set; }
      
        public int iOtpLength { get; set; }
      
        public string strOtpSubmissionJSCode { get; set; }
      
        public bool isNetbankingLogin { get; set; }
      
        public string jsCodeToCustomerId { get; set; }
      
        public string setJsCodeToCustomerId { get; set; }
      
        public bool isNetbankingPage { get; set; }
      
        public bool IsAutoOtpSubmitDisabled { get; set; }
      
        public bool IsGenricOtpFillingJsCodeDisabled { get; set; }
        public string strPageCustomizationCodeForIOS { get; set; }
        public bool IsOtpCheckEnabled { get; set; }
        public string strOtpCheckJsCode { get; set; }
    }
}
