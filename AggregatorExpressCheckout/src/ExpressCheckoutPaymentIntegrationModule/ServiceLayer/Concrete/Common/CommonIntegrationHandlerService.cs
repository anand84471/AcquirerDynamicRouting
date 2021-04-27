using Core.Utilities;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Concrete.Common
{
    public class CommonIntegrationHandlerService : ICommonIntegrationHandlerService
    {
        private readonly IOrderRepo orderRepo;
        private readonly IMerchantRepo merchantRepo;

        public CommonIntegrationHandlerService(IOrderRepo orderRepo, IMerchantRepo merchantRepo)
        {
            this.orderRepo = orderRepo;
            this.merchantRepo = merchantRepo;
        }

        //public Task<List<EnumGateway>> GetGatewaysAccordingToDynamicRouting(int merchantId)
        //{

        //}

        public async Task<string> UpdateTransactionResponseAndCreateResponseForMerchant(FinalResponseHelperDto finalResponseHelperDto)
        {
            if (finalResponseHelperDto.AggPaymentId > 0)
            {
                var orderStepDto = new OrderStepDto
                {
                    PaymentId = finalResponseHelperDto.AggPaymentId,
                    RequestType = 3,
                    ResponseReceivedFromGateway = finalResponseHelperDto.FinalResponseFromGateway
                };
                await this.orderRepo.UpdateOrderPaymentStepDetails(orderStepDto);
            }
            if (!string.IsNullOrEmpty(finalResponseHelperDto.GatewayPaymentId))
            {
                var updatedOrderPaymentDetails = new OrderPaymentDetailsDto
                {
                    GatewayPaymentId = finalResponseHelperDto.GatewayPaymentId,
                    AggPaymentId = finalResponseHelperDto.AggPaymentId
                };
                await this.orderRepo.UpdateOrderPaymentDetails(updatedOrderPaymentDetails);
            }
            var orderToBeUpdated = new UpdateOrderDetailsDto
            {
                AggOrderId = finalResponseHelperDto.AggOrderId,
                OrderStatus = (short)finalResponseHelperDto.OrderStatus,
                OrderResponseCode = finalResponseHelperDto.OrderResponseCode
            };
            await this.orderRepo.UpdateOrderDetails(orderToBeUpdated);

            var order = await this.orderRepo.GetOrderDetails(finalResponseHelperDto.AggOrderId);

            var base64Converted = GenericUtility.GetBase64FromObject(order);

            var merchantDto = await this.merchantRepo.GetMerchantData(finalResponseHelperDto.MerchantId);

            var shaGenerated = GenericUtility.GetSHAGenerated(base64Converted, merchantDto.SecureSeret);

            return shaGenerated;

        }
    }
}
