using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO
{
    public class GlobalBinDataDto
    {
        public string bin { get; set; }
        public string issuerName { get; set; }
        public string issuerId { get; set; }
        public string cardType { get; set; }
        public string cardTypeId { get; set; }
        public string cardScheme { get; set; }
        public string cardSchemeId { get; set; }
        public string level { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
    }
}
