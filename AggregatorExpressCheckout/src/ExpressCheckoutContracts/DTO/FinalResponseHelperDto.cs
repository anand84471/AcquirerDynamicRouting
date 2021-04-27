using ExpressCheckoutContracts.Enums;

namespace ExpressCheckoutContracts.DTO
{
    public class FinalResponseHelperDto
    {
        public string FinalResponseFromGateway { get; set; }

        public long AggOrderId { get; set; }

        public long AggPaymentId { get; set; }

        public string GatewayPaymentId { get; set; }

        public EnumOrderStatus OrderStatus { get; set; }

        public int OrderResponseCode { get; set; }

        public int MerchantId { get; set; }
    }
}
