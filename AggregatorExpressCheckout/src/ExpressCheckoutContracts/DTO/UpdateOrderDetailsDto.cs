using ExpressCheckoutContracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO
{
  public class UpdateOrderDetailsDto
    {
        public long AggOrderId { get; set; }
        public long? RefundAmount { get; set; }
        public long? PaymentId { get; set; }

        public int? GatewayId { get => (int?)GatewayCode; set => GatewayCode = (EnumGateway)value; }
       
        public EnumGateway? GatewayCode { get; set; }
        public int? OrderResponseCode { get; set; }

        public short RequestType { get => (short)RequestTypeCode; set => RequestTypeCode = (EnumRequestType)value; }
        public EnumRequestType RequestTypeCode { get; set; }

        public short? OrderStatus { get => (short?)OrderStatusCode; set => OrderStatusCode = (EnumOrderStatus)value; }

        public EnumOrderStatus? OrderStatusCode { get; set; }
    }
}
