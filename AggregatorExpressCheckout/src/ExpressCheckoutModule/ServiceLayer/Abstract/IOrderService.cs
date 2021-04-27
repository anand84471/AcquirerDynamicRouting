using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ServiceLayer.Abstract
{
    public interface IOrderService

    {
        Task<long> CreateOrder(OrderDetailsDto orderDetailsDto);

        Task<bool> CheckMerchantOrderIdExist(MerchantDto merchantDto);
        Task<OrderDetailsDto> GetOrderDetails(long AggOrderId);
        Task<OrderDetailsDto> DoOrderInquiry(OrderDetailsDto parentOrderDetailsDto);
        Task<bool> UpdatePurchaseOrder(OrderDetailsDto orderDetailsDto);
     
        Task<string> DoPayment(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetails);
        //Task<OrderDetailsDto> DoRefund(OrderDetailsDto parentOrderDetailsDto, OrderDetailsDto refundRequestDto);
        Task<OrderPaymentDetailsDto> GetAggregatorOrderDetailsByGatewayOrderIdAsync(EnumGateway enumGateway, string gatewayOrderId);
        Task<OrderDetailsDto> DoOrderRefund(OrderDetailsDto parentOrderDetailsDto, OrderDetailsDto refundRequestDto);
    }
}