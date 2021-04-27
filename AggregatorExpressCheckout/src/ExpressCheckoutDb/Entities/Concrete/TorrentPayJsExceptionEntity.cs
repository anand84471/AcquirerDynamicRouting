using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutDb.Entities
{
    public class TorrentPayJsExceptionEntity
    {
        
        public string m_strExceptionUrl { get; set; }
        
        public string m_strExceptionMessage { get; set; }
        
        public string m_strHtmlOfExceptionUrl { get; set; }
        
        public int m_iClientTypeId { get; set; }
        
        public string m_strClientVersion { get; set; }
        
        public string m_strExecutedJsCode { get; set; }
        
        public int m_iExecutedJsCodeTypeId { get; set; }
    }
}
