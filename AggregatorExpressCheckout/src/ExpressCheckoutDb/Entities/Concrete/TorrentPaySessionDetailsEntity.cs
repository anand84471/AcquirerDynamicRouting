using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutDb.Entities
{
    public class TorrentPaySessionDetailsEntity
    {
        public int m_iMerchantid;

        public string m_strTransactionId;
  
        public int m_iClientId;

        public string m_strCustomerEmail;

        public string m_strCustomerPhoneNo;
 
        public string m_strCustomeId;

        public string m_strRemarks;
    
        public string m_strPaymentStartUrl;
      
        public string m_strPaymentReturnUrls;
   
        public string m_strPostData;
  
        public string m_strCustomParameters;
 
        public bool? m_bISPaymentSuccessful;
   
        public string m_strLastVisitedUrl;

        public bool? m_bIsBackPressed;
   
        public bool? m_bIsTransactionWasCompleted;
    
        public bool? m_bIsOtpDetected;
  
        public bool? m_bHaveSmsPermission;
    
        public bool? m_bIsFetchedOtpDetails;
      
        public bool? m_bIsOtpApproved;
      
        public int? m_iErrorCode;
    
        public string m_strErrorMessage;
       
        public string m_strBrowserSessionStartTime;
  
        public string m_strBrowserSessionFinishTime;
    
        public string m_strOrderId;
   
        public int m_iThemeId;
       
        public int m_iLanguageId;
      
        public DateTime? m_dtPaymentStatusChangeDate;
    }
}
