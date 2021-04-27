using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO
{
   public class PaymentDataDto
    {

       
        public long AggOrderId { get; set; }

       
        public long PaymentId { get; set; }

       
        public short RequestType { get; set; }

       
        public int GatewayId { get; set; }

        
        public int RequestCount { get; set; }

      
        public int PaymentMode { get; set; }

        
        public string OrderIdGeneratedByGateway { get; set; }

       
        public string PaymentIdGeneratedByGateway { get; set; }

        public string RequestSendToGateWay { get; set; }

       
        public string ResponseRecievedFromGateway { get; set; }

        
        public string Status { get; set; }

       
        public string ResponseCode { get; set; }

       
        public string ErrorCode { get; set; }

        
        public string ErrorDescription { get; set; }
    }

}
