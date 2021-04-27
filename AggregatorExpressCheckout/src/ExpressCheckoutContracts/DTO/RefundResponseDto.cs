using ExpressCheckoutContracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO
{
   public class RefundResponseDto
    {
       
        public bool IsRequestSend { get; set; }
        public bool IsResponseReceived { get; set; }
        public string RequestSendToGateway { get; set; }

        public string ResponseRecievedFromGateway { get; set; }

        public string ResponseCodeRecievedFromateway { get; set; }

        public string GatewayPaymentId { get; set; }

        public string GatewayOrderID { get; set; }

        public EnumOrderStatus DependentPaymentTxnOrderStatusCode { get; set; }

        public int DependentPaymentTxnOrderResponseCode { get; set; }
      
        public bool IsParentOrderIdTobeUpdate { get; set; }

        public short ParentOrderIdOrderStatusTobeUpdate { get; set; }

        public int ParentOrderIdReponseCodeTobeUpdate { get; set; }
      

    }
}
