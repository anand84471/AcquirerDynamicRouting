using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO
{
   public  class GatewayDto
    {
        public int GatewayId { get; set; }

        public string GatewayName { get; set; }

       
        public string OrderUrl { get; set; }

       
        public string CardPaymentUrl { get; set; }

      
        public string NetbankingPaymentUrl { get; set; }

        public string InquiryUrl { get; set; }

        public string RefundUrl { get; set; }

        public string InquiryRefundUrl { get; set; }

        public string ResponseReturnedUrl { get; set; }



    }
}
