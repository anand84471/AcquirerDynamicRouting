using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Requests
{
    public class TorrentPayChangeTxnStatusRequest
    {
        [JsonProperty("txn_status")]
        public bool m_TorrentPayTxnStatus;
        [JsonProperty("unique_merchant_txn_id")]
        public string m_strUniqueMerchantTxnId;
        [JsonProperty("txn_id")]
        public int m_iMerhcantId;
    }
}
