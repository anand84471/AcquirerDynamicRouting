using System.Collections.Generic;

namespace ExpressCheckoutContracts.ExternalApis.Responses
{
    public class GlobabBinCardInfoResponse
    {
        public List<GlobalBinsData> GlobalBinsData { get; set; }
        public ResultInfo resultInfo { get; set; }
    }

    public class GlobalBinsData
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

    public class ResultInfo
    {
        public int responseCode { get; set; }
        public string totalBins { get; set; }
        public string binsNotFound { get; set; }
    }
}