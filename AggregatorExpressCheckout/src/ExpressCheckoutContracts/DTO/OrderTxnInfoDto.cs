using ExpressCheckoutContracts.Enums;

namespace ExpressCheckoutContracts.DTO
{
    public class OrderTxnInfoDto
    {
        public long AggOrderId { get; set; }

        public long Amount { get; set; }

        public int CurrencyId {
            get { return (int)CurrencyCode; }
            set
            {
                CurrencyCode = (EnumCurrency)value;
            }
        }

        public int PreferredGatewayId {
            get {return PrefferedGatewayCode.HasValue ? (int)PrefferedGatewayCode : 0;}
            set
            {
                PrefferedGatewayCode = (EnumGateway)value;

            }

        }

        public short TxnType
        {
            get { return TxnTypeCode.HasValue ? (short)TxnTypeCode : (short)0; }
            set
            {
                TxnTypeCode = (EnumTxnType)value;

            }

        }
        public string OrderDesc { get; set; }

        public long ParentOrderId { get; set; }

        public short OrderStatus {
            get { return OrderStatusCode.HasValue ? (short)OrderStatusCode : (short)0; }
            set { OrderStatusCode = (EnumOrderStatus)value; }
          
        }

        public int OrderResponseCode { get; set; }

        public EnumCurrency CurrencyCode { get; set; }

        public EnumGateway? PrefferedGatewayCode { get; set; }
      
        public EnumTxnType? TxnTypeCode { get; set; }

        public EnumOrderStatus? OrderStatusCode { get; set; }

        public long RefundAmount { get; set; }

       
        public short ParentOrderStatus
        {
            get { return ParentOrderStatusCode.HasValue ? (short)ParentOrderStatusCode : (short)0; }
            set { ParentOrderStatusCode = (EnumOrderStatus)value; }
        }

       
        public int ParentOrderResponseCode { get; set; }


        public EnumOrderStatus? ParentOrderStatusCode { get; set; }

    }
}