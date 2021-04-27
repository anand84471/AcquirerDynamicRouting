using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.Enums
{
    public enum EnumOrderStatus
    {
        NONE = 0,
        ORDER_CREATED =1,
        PENDNG_AUTHENTICATION=2,
        CHARGED=3,
        AUTHENTICATION_FAILED=4,
        AUTHORIZATION_FAILED=5,
        EDGE_DECLINED=6,
        AUTHORIZING=7,
        STARTED=8,
        AUTO_REFUNDED=9,
        GATEWAY_ERROR=10,
        INQUIRY_COMPLETED=11,
        REFUNDED=12,
        FAILED=13,
        PENDING=14

    }
}
