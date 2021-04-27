using ExpressCheckoutContracts.Enums;

namespace ExpressCheckoutContracts.DTO
{
    public class OrderStepDto
    {
        public long PaymentId { get; set; }

        public short RequestType { get => (short)RequestTypeCode; set => RequestTypeCode = (EnumRequestType)value; }
        public EnumRequestType RequestTypeCode { get; set; }

        public int RequestCount { get; set; }

        public string RequestSendToGateway { get; set; }

        public string ResponseReceivedFromGateway { get; set; }

        public string ResponseCodeReceivedFromGateway { get; set; }

    }
}
