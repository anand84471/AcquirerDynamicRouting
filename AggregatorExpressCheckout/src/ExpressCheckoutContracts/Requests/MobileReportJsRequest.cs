using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Requests
{
    public class MobileReportJsRequest
    {
        [JsonProperty("url")]
        public string m_strExceptionUrl;
        [JsonProperty("exception")]
        public string m_strExceptionMessage;
        [JsonProperty("html")]
        public string m_strHtmlOfExceptionUrl;
        [JsonProperty("client_type_id")]
        public int m_iClientTypeId;
        [JsonProperty("client_version")]
        public string m_strClientVersion;
        [JsonProperty("js_code_applied")]
        public string m_strExecutedJsCode;
        [JsonProperty("js_code_type_id")]
        public int m_iExecutedJsCodeTypeId;
    }
}
