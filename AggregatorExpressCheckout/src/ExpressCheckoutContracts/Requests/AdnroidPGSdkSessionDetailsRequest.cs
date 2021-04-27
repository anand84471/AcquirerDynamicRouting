using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ExpressCheckoutContracts.Requests
{
    public class AdnroidPGSdkSessionDetailsRequest
    {
        [JsonProperty("merchant_id")]
        public int m_iMerchantid;
        [JsonProperty("transaction_id")]
        public string m_strTransactionId;
        [JsonProperty("client_type_id")]
        public int m_iClientId;
        [JsonProperty("customer_email")]
        public string m_strCustomerEmail;
        [JsonProperty("customer_phone")]
        public string m_strCustomerPhoneNo;
        [JsonProperty("customer_id")]
        public string m_strCustomeId;
        [JsonProperty("payment_remarks")]
        public string m_strRemarks;
        [JsonProperty("payment_start_url")]
        public string m_strPaymentStartUrl;
        [JsonProperty("payment_return_urls")]
        public string m_strPaymentReturnUrls;
        [JsonProperty("post_data")]
        public string m_strPostData;
        [JsonProperty("custom_parameters")]
        public string m_strCustomParameters;
        [JsonProperty("is_payment_successful")]
        public bool? m_bIsPaymentSuccessful;
        [JsonProperty("last_visited_url")]
        public string m_strLastVisitedUrl;
        [JsonProperty("is_back_pressed")]
        public bool? m_bIsBackPressed;
        [JsonProperty("is_transaction_completed")]
        public bool? m_bIsTransactionWasCompleted;
        [JsonProperty("is_otp_detected")]
        public bool? m_bIsOtpDetected;
        [JsonProperty("have_sms_permission")]
        public bool? m_bHaveSmsPermission;
        [JsonProperty("is_fetched_url_details")]
        public bool? m_bIsFetchedOtpDetails;
        [JsonProperty("is_otp_approved")]
        public bool? m_bIsOtpApproved;
        [JsonProperty("error_code")]
        public int? m_iErrorCode;
        [JsonProperty("error_message")]
        public string m_strErrorMessage;
        [JsonProperty("browser_session_starttime")]
        public string m_strBrowserSessionStartTime;
        [JsonProperty("browser_session_finish_time")]
        public string m_strBrowserSessionFinishTime;
        [JsonProperty("order_id")]
        public string m_strOrderId;
        [JsonProperty("theme_id")]
        public string m_iThemeId;
        [JsonProperty("language_id")]
        public string m_iLanguageId;
    }
}
