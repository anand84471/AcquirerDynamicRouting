using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ExpressCheckoutContracts.DTO
{
    public class BankOTPUrlDTO
    {
        [JsonProperty("BANK_NAME", NullValueHandling = NullValueHandling.Ignore)]
        public string bankName { get; set; }

        [JsonProperty("URL", NullValueHandling = NullValueHandling.Ignore)]
        public string url { get; set; }

        [JsonProperty("IS_PAGE_TO_BE_RESPONSIVE", NullValueHandling = NullValueHandling.Ignore)]
        public bool isPageToBeResponsive { get; set; }

        [JsonProperty("IS_OTP_URL", NullValueHandling = NullValueHandling.Ignore)]
        public bool isOTPUrl { get; set; }
        [JsonProperty("IS_NET_BANKING_SUBMISSION_PAGE", NullValueHandling = NullValueHandling.Ignore)]
        public bool isPageNetBankingSubmissionPage { get; set; }
        [JsonProperty("PAGE_CUSTOMIZATION_JS_CODE", NullValueHandling = NullValueHandling.Ignore)]
        public string strPageCustomizationJsCode { get; set; }
        [JsonProperty("NET_BANKING_PAGE_SUBMISSION_JS_CODE", NullValueHandling = NullValueHandling.Ignore)]
        public string strNetBankingSubmissionJsCode { get; set; }
        [JsonProperty("OTP_LENGTH", NullValueHandling = NullValueHandling.Ignore)]
        public int iOtpLength { get; set; }
        [JsonProperty("OTP_BANKING_SUBMISSION_JSCODE", NullValueHandling = NullValueHandling.Ignore)]
        public string strOtpSubmissionJSCode { get; set; }
        [JsonProperty("IS_NETBANKING_LOGIN", NullValueHandling = NullValueHandling.Ignore)]
        public bool isNetbankingLogin { get; set; }
        [JsonProperty("GET_JSCODE_TO_CUSTOMER_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string jsCodeToCustomerId { get; set; }
        [JsonProperty("IS_NETBANKING_PAGE", NullValueHandling = NullValueHandling.Ignore)]
        public bool isNetbankingPage { get; set; }
        [JsonProperty("SET_JSCODE_TO_CUSTOMER_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string setJsCodeToCustomerId { get; set; }
        [JsonProperty("IS_AUTO_OTP_SUBMIT_DISABLED", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsAutoOtpSubmitDisabled { get; set; }
        [JsonProperty("IS_GENERIC_OTP_FILLING_JS_CODE_DISABLED", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsGenricOtpFillingJsCodeDisabled { get; set; }
        [JsonProperty("IS_OTP_PAGE_CHECK_ENABLED", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsOtpCheckEnabled { get; set; }
        [JsonProperty("OTP_PAGE_CHECK_JS_CODE", NullValueHandling = NullValueHandling.Ignore)]
        public string strOtpCheckJsCode { get; set; }


        public BankOTPUrlDTO()
        {

        }

        public BankOTPUrlDTO(string bankName, string url, bool isPageToBeResponsive, bool isOTPUrl)
        {
            this.bankName = bankName;
            this.url = url;
            this.isPageToBeResponsive = isPageToBeResponsive;
            this.isOTPUrl = isOTPUrl;
        }
        public BankOTPUrlDTO(string bankName, string url, bool isPageToBeResponsive, bool isOTPUrl, bool isPageNetBankingSubmissionPage, string PageCustomizationJsCode,
                             string NetBankingPageSubmissionCode, int OtpLength,string strOtpSubmissionJSCode,string jsCodeToCustomerId,bool isNetbankingLogin,bool isNetbbankingPage,string setJsCodeToCustomerId,
                             bool isAutoSubmitDisabled,bool isGenericOtpFillingJsCodeDisabled,
                             bool isOtpCheckEnabled,string otpCheckJsCode)
        {
            this.bankName = bankName;
            this.url = url;
            this.isPageToBeResponsive = isPageToBeResponsive;
            this.isOTPUrl = isOTPUrl;
            this.isPageNetBankingSubmissionPage = isPageNetBankingSubmissionPage;
            this.strPageCustomizationJsCode = PageCustomizationJsCode;
            this.strNetBankingSubmissionJsCode = NetBankingPageSubmissionCode;
            this.iOtpLength = OtpLength;
            this.isNetbankingLogin = isNetbankingLogin;
            this.isNetbankingPage = isNetbankingPage;
            this.strOtpSubmissionJSCode = strOtpSubmissionJSCode;
            this.jsCodeToCustomerId = jsCodeToCustomerId;
            this.setJsCodeToCustomerId = setJsCodeToCustomerId;
            //Changed By Anand
            this.IsAutoOtpSubmitDisabled = isAutoSubmitDisabled;
            this.IsGenricOtpFillingJsCodeDisabled = isGenericOtpFillingJsCodeDisabled;
            this.IsOtpCheckEnabled = isOtpCheckEnabled;
            this.strOtpCheckJsCode = otpCheckJsCode;

        }
    }
}
