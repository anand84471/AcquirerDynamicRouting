using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutDb.Entities.Concrete
{
    public class TorrentPayChangeTxnStatusEntity
    {
        public string m_iMerchantId { get; set; }
        public bool m_bTransactionStatus { get; set; }
        public string m_strTransactionId { get; set; }
    }
}
