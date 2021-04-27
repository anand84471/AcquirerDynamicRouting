using ExpressCheckoutContracts.Enums;

namespace ExpressCheckoutContracts.DTO
{
    public class OrderPaymentDetailsDto
    {
        public long AggOrderId { get; set; }
        public long AggPaymentId { get; set; }
        public int GatewayId { get; set; }    
        public int? PaymentModeId { get; set; }       
        public short RequestType { get => (short)RequestTypeCode; set => RequestTypeCode=(EnumRequestType)value; }
        public EnumRequestType RequestTypeCode { get; set; }

        public string GatewayOrderID { get; set; }

        public string GatewayPaymentId { get; set; }

        public short? PaymentStatus { get => (short?)PaymentStatusCode; set => PaymentStatusCode = (EnumOrderStatus)value; }

        public EnumOrderStatus? PaymentStatusCode { get; set; }

        public int? PaymentResponseCode { get; set; }
    }
}
