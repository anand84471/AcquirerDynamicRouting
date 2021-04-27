using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using ExpressCheckout.BusinessLayer.Abstract;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.Razorpay;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using ExpressCheckoutPaymentIntegrationModule.Factory.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ExpressCheckout.BusinessLayer.Concrete
{
    public class RazorpayValidation : IRazorpayValidation
    {

        private readonly IOrderService orderService;
        private readonly IPaymentIntegrationModuleFactory _paymentIntegrationModuleFactory;
        private readonly IGatewayService _gateWayService;
        private readonly IMerchantService merchantService;
        private readonly ICommonIntegrationHandlerService commonIntegrationHandlerService;

        public RazorpayValidation(IOrderService orderService, IGatewayService _gateWayService,
                                  IPaymentIntegrationModuleFactory _paymentIntegrationModuleFactory, IMerchantService merchantService,
                                  ICommonIntegrationHandlerService commonIntegrationHandlerService)
        {
            this.orderService = orderService;
            this._gateWayService = _gateWayService;
            this._paymentIntegrationModuleFactory = _paymentIntegrationModuleFactory;
            this.merchantService = merchantService;
            this.commonIntegrationHandlerService = commonIntegrationHandlerService;
        }

        private async Task<OrderPaymentDetailsDto> GetOrderPaymentDetails(EnumGateway enumGateway, string orderId)
        {
            return await this.orderService.GetAggregatorOrderDetailsByGatewayOrderIdAsync(enumGateway, orderId);
        }

        public async Task<string> CompleteRazorpayTransaction(IFormCollection formCollection)
        {
            string paymentID = formCollection["razorpay_payment_id"];
            string gatewayorderid = formCollection["razorpay_order_id"];

            if (string.IsNullOrEmpty(paymentID) || string.IsNullOrEmpty(gatewayorderid))
            {
                var metaData = formCollection["error[metadata]"];
                RazorpayFinalResponseMetaData razorpayFinalResponseMetaData = JsonConvert.DeserializeObject<RazorpayFinalResponseMetaData>(metaData);
                gatewayorderid = razorpayFinalResponseMetaData.OrderId;
                paymentID = razorpayFinalResponseMetaData.PaymentId;
            }

            var orderPaymentDetails = await this.GetOrderPaymentDetails(EnumGateway.RAZOR_PAY, gatewayorderid);

            var orderDetails = await this.orderService.GetOrderDetails(orderPaymentDetails.AggOrderId);

            MerchantDto merchantDto = await merchantService.GetMerchantData(orderDetails.MerchantDto.MerchantId);
            if (merchantDto == null)
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
            }

            var inquiryResposneFromRazorpay = await DoInquiryAtRazorPay(paymentID, orderDetails);

            var finalResponseHelperDto = new FinalResponseHelperDto
            {
                AggOrderId = orderPaymentDetails.AggOrderId,
                AggPaymentId = orderPaymentDetails.AggPaymentId,
                GatewayPaymentId = paymentID,
                FinalResponseFromGateway = inquiryResposneFromRazorpay.ResponseRecievedFromGateway,
                OrderStatus = inquiryResposneFromRazorpay.ResponseCodeRecievedFromGateway.Equals("captured") ? EnumOrderStatus.CHARGED : EnumOrderStatus.AUTHORIZING,
                MerchantId = merchantDto.MerchantId,
                OrderResponseCode = inquiryResposneFromRazorpay.ResponseCodeRecievedFromGateway.Equals("captured") ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE
            };

            return await this.commonIntegrationHandlerService.UpdateTransactionResponseAndCreateResponseForMerchant(finalResponseHelperDto);
        }

        private async Task<InquiryResponseDto> DoInquiryAtRazorPay(string paymentIDFromGateway, OrderDetailsDto orderDetails)
        {
            var orderDetailsDto = new OrderDetailsDto
            {
                PaymentDataDto = new PaymentDataDto
                {
                    PaymentIdGeneratedByGateway = paymentIDFromGateway
                }
            };

            var taskGatewayDto = _gateWayService.GetGatewayDetails((int)EnumGateway.RAZOR_PAY);
            var taskMerchantGatewayConfigurationMappingDto = merchantService.GetMerchantPaymentGatewayConfigurationDetails(orderDetails.MerchantDto.MerchantId, (int)EnumGateway.RAZOR_PAY);
            var paymentGatewayHandlerService = _paymentIntegrationModuleFactory.GetPaymentGatewayHandlerService(EnumGateway.RAZOR_PAY);

            return await paymentGatewayHandlerService.DoInquiryOfPurchase(orderDetailsDto, await taskGatewayDto, await taskMerchantGatewayConfigurationMappingDto);
        }
    }
}
