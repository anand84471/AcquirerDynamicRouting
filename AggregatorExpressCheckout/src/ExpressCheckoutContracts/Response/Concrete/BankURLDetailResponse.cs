using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ExpressCheckoutContracts.Response.Concrete
{
    public class BankURLDetailResponse
    {
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
    }
}
