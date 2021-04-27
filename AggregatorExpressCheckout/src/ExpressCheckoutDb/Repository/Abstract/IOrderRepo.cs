using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests;
using System.Threading.Tasks;

namespace ExpressCheckoutDb.Repository.Abstract
{
    public interface IOrderRepo
    {
        Task<long> CreateOrder(OrderDetailsDto orderDetailsDto);

        Task<bool> CheckMerchantOrderIdExist(MerchantDto merchantDto);

        Task<OrderDetailsDto> GetOrderDetails(long AggOrderId);

        Task<long> InsertOrderPaymentDetailsAndStepDetails(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetails);
        Task InsertOrderPaymentStepDetails(OrderStepDto orderStepDto);
        Task<long> InsertPaymentDetails(OrderPaymentDetailsDto orderPaymentDetailsDto);
        Task<bool> UpdatePurchaseOrder(OrderDetailsDto orderupdateDetailsDto);
        Task<bool> UpdateOrderPaymentDetails(OrderPaymentDetailsDto orderPaymentDetailsDto);
        Task<bool> UpdateOrderDetails(UpdateOrderDetailsDto updateOrderDetailsDto);
        Task<bool> UpdateOrderPaymentStepDetails(OrderStepDto orderStepDto);
        Task<bool> UpdateBinDataInOrderTbl(OrderDetailsDto orderDetailsDto,long aggOrderId);

        Task<OrderPaymentDetailsDto> GetAggregatorOrderDetailsByGatewayOrderIdAsync(EnumGateway enumGateway, string gatewayOrderId);
        Task<bool> UpdateDependentOrderDetails(OrderPaymentDetailsDto orderPaymentDetailsDto,
             OrderStepDto orderStepDto, UpdateOrderDetailsDto updateOrderDetailsDto);

        Task<bool> UpdateParentTransactionOrderDetails(OrderPaymentDetailsDto orderPaymentDetailsDto,
            OrderStepDto orderStepDto, UpdateOrderDetailsDto updateOrderDetailsDto, UpdateOrderDetailsDto updatePurchaseOrderDetailsDto=null);
    }
}