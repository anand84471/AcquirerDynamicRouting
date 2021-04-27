using System;
using System.Collections.Generic;
using System.Text;
using ExpressCheckoutContracts.Response.Abstract;
using ExpressCheckoutContracts.Response.Concrete;
using Newtonsoft.Json;

namespace ExpressCheckoutContracts.DTO
{
    public class BankUrlOtpResponseDTO  : MasterResponse
    {
        [JsonProperty("url_data")]
        public BankURLDetailResponse[] m_lsUrlDetailsModel { get; set; }
    }
}
