using AutoMapper;
using Core.Constants;
using Core.Utilities;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.Razorpay;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutPaymentIntegrationModule.ApiClients.Abstract;
using ExpressCheckoutPaymentIntegrationModule.Factory.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;
using System.Threading.Tasks;
using ExpressCheckoutDb.Repository.Abstract;
using Newtonsoft.Json;
using Core.Features.ExceptionHandling.Concrete;
using PinePGController.ExceptionHandling.CustomExceptions;

namespace ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Concrete.Razorpay
{
    public class RazorpayIntegrationHandlerService : IGatewayIntegrationHandlerService
    {
        private readonly IMapper mapper;
        private readonly IPaymentIntegrationApiClient apiClient;
        private readonly IPaymentIntegrationModuleFactory paymentIntegrationModuleFactory;
        private readonly IOrderRepo _orderRepo;

        public RazorpayIntegrationHandlerService(IMapper mapper, IPaymentIntegrationApiClient apiClient, IPaymentIntegrationModuleFactory paymentIntegrationModuleFactory
                                                , IOrderRepo _orderRepo)
        {
            this.mapper = mapper;
            this.apiClient = apiClient;
            this.paymentIntegrationModuleFactory = paymentIntegrationModuleFactory;
            this._orderRepo = _orderRepo;
        }

        private async Task<RazorpayOrderResponseDto> CreateOrderAtRazorpay(RazorpayOrderRequestDto razorpayOrderRequestDto, OrderDetailsDto orderDetailsDto)
        {
            var razorPayOrderResponse = await this.apiClient.CreateOrderAtRazorpay(razorpayOrderRequestDto, orderDetailsDto);
            return razorPayOrderResponse;
        }

        private RazorpayOrderRequestDto CreateOrderRequest(OrderDetailsDto orderDetailsDto)
        {
            return this.mapper.Map<RazorpayOrderRequestDto>(orderDetailsDto);
        }

        public async Task<string> DoPurchase(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetailsDto)
        {
            var razorpayOrderRequest = CreateOrderRequest(orderDetailsDto);
            var razorpayOrderResponse = await CreateOrderAtRazorpay(razorpayOrderRequest, orderDetailsDto);
            if (razorpayOrderResponse == null)
            {
                throw new OrderException(ResponseCodeConstants.ORDER_CREATION_FAILED_AT_AGGREGATOR);
            }
            var orderStepDto = new OrderStepDto
            {
                PaymentId = orderDetailsDto.PaymentDataDto.PaymentId,
                RequestSendToGateway = JsonConvert.SerializeObject(razorpayOrderRequest),
                RequestType = 2,
                ResponseReceivedFromGateway = JsonConvert.SerializeObject(razorpayOrderResponse),
            };
            await this._orderRepo.InsertOrderPaymentStepDetails(orderStepDto);
            var updatedOrderPaymentDetail = new OrderPaymentDetailsDto
            {
                AggPaymentId = orderDetailsDto.PaymentDataDto.PaymentId,
                GatewayOrderID = razorpayOrderResponse.Id
            };
            if (!await this._orderRepo.UpdateOrderPaymentDetails(updatedOrderPaymentDetail))
            {
                throw new DBException(ResponseCodeConstants.FAILURE);
            }
            var orderToBeUpdated = new UpdateOrderDetailsDto
            {
                AggOrderId = orderDetailsDto.OrderTxnInfoDto.AggOrderId,
                PaymentId = orderDetailsDto.PaymentDataDto.PaymentId,
                GatewayId = (int)EnumGateway.RAZOR_PAY
            };
            if (!await this._orderRepo.UpdateOrderDetails(orderToBeUpdated))
            {
                throw new DBException(ResponseCodeConstants.FAILURE);
            }
            var razorpayPaymentModeHandlerService = this.paymentIntegrationModuleFactory.GetRazorpayPaymentModeHandlerService(doPaymentRequest.PaymentDetailsRequest.PaymentMode);
            return await razorpayPaymentModeHandlerService.DoPayment(doPaymentRequest, orderDetailsDto, razorpayOrderResponse);
        }


        public async Task<InquiryResponseDto> DoInquiryOfPurchase(OrderDetailsDto orderDetailsDto, GatewayDto gatewayDto, MerchantGatewayConfigurationMappingDto merchantGatewayConfigurationMappingDto)
        {
            string razorPaymentId = orderDetailsDto.PaymentDataDto.PaymentIdGeneratedByGateway;
            string inquiryUrl = gatewayDto.InquiryUrl;
            string PaymentURL = inquiryUrl + "/" + razorPaymentId + "/";
            string apiUsername = merchantGatewayConfigurationMappingDto.MerchantIdIssuedByGatewayToMerchant;
            string apiPassword = merchantGatewayConfigurationMappingDto.PasswordIssuedByGatewayToMerchant;

            InquiryResponseDto inquiryResponseDto = new InquiryResponseDto();
            inquiryResponseDto.IsRequestSend = true;
            inquiryResponseDto.RequestSendToGateway = GenericUtility.ConvertObjectToJsonString<string>(PaymentURL);
            RazorPayInquiryResponseDto razorPayInquiryResponseDto = await this.apiClient.DoInquiryOnRazorPay(PaymentURL, apiUsername, apiPassword);
            inquiryResponseDto.IsResponseReceived = true;
            if (razorPayInquiryResponseDto != null)
            {
                inquiryResponseDto.ResponseRecievedFromGateway = GenericUtility.ConvertObjectToJsonString<RazorPayInquiryResponseDto>(razorPayInquiryResponseDto);
                inquiryResponseDto.ResponseCodeRecievedFromGateway = razorPayInquiryResponseDto.Status;
                inquiryResponseDto.DependentPaymentTxnOrderResponseCode = ResponseCodeConstants.SUCCESS;
                inquiryResponseDto.DependentPaymentTxnOrderStatusCode = EnumOrderStatus.INQUIRY_COMPLETED;

                if (!string.IsNullOrEmpty(razorPayInquiryResponseDto.Status) &&
                    razorPayInquiryResponseDto.Status.Equals("captured", System.StringComparison.OrdinalIgnoreCase))
                {
                    inquiryResponseDto.IsParentOrderIdTobeUpdate = true;
                    inquiryResponseDto.ParentOrderIdOrderStatusTobeUpdate = (int)EnumOrderStatus.CHARGED;
                    inquiryResponseDto.ParentOrderIdReponseCodeTobeUpdate = ResponseCodeConstants.SUCCESS;
                    inquiryResponseDto.ParentOrderIdResponseRecievedFromGatewayToBeUpdate = inquiryResponseDto.ResponseRecievedFromGateway;

                }
            }
            else
            {
                inquiryResponseDto.DependentPaymentTxnOrderResponseCode = ResponseCodeConstants.FAILURE;
                inquiryResponseDto.DependentPaymentTxnOrderStatusCode = EnumOrderStatus.GATEWAY_ERROR;


            }
            return inquiryResponseDto;
        }

        public async Task<RefundResponseDto> DoRefund(OrderDetailsDto parentOrderDetailsDto,long AmountToBeRefunded, GatewayDto gatewayDto, MerchantGatewayConfigurationMappingDto merchantGatewayConfigurationMappingDto)
        {
            string razorPaymentId = parentOrderDetailsDto.PaymentDataDto.PaymentIdGeneratedByGateway;
            string refundUrl = gatewayDto.RefundUrl;
            string PaymentURL = refundUrl + "/" + razorPaymentId + "/refund";
            string apiUsername = merchantGatewayConfigurationMappingDto.MerchantIdIssuedByGatewayToMerchant;
            string apiPassword = merchantGatewayConfigurationMappingDto.PasswordIssuedByGatewayToMerchant;
            RazorPayRefundRequestDto razorPayRefundRequestDto = new RazorPayRefundRequestDto();
            razorPayRefundRequestDto.amount = AmountToBeRefunded;

            RefundResponseDto refundResponseDto = new RefundResponseDto();
            refundResponseDto.IsRequestSend = true;

            refundResponseDto.RequestSendToGateway = GenericUtility.ConvertObjectToJsonString<string>(PaymentURL);
            RazorPayRefundResponseDto razorPayRefundResponseDto = await this.apiClient.DoRefundOnRazorPay(PaymentURL, razorPayRefundRequestDto ,apiUsername, apiPassword);
            refundResponseDto.IsResponseReceived = true;
            if (razorPayRefundResponseDto != null)
            {
                refundResponseDto.ResponseRecievedFromGateway = GenericUtility.ConvertObjectToJsonString<RazorPayRefundResponseDto>(razorPayRefundResponseDto);
                refundResponseDto.ResponseCodeRecievedFromateway = razorPayRefundResponseDto.Status;

                /*
                 * 
                    pending: This state indicates that Razorpay is attempting to process the refund.
                    processed: This is the terminal state of the refund.
  
                 * 
                 * 
                 */
                if (!string.IsNullOrEmpty(razorPayRefundResponseDto.Status) &&
                    razorPayRefundResponseDto.Status.Equals("processed", System.StringComparison.OrdinalIgnoreCase))
                {
                    refundResponseDto.IsParentOrderIdTobeUpdate = true;
                    refundResponseDto.ParentOrderIdOrderStatusTobeUpdate = (int)EnumOrderStatus.REFUNDED;
                    refundResponseDto.ParentOrderIdReponseCodeTobeUpdate = ResponseCodeConstants.SUCCESS;
                  
                    refundResponseDto.GatewayPaymentId = razorPayRefundResponseDto.id;
                    refundResponseDto.GatewayOrderID = null;
                    refundResponseDto.DependentPaymentTxnOrderResponseCode = ResponseCodeConstants.SUCCESS;
                    refundResponseDto.DependentPaymentTxnOrderStatusCode = EnumOrderStatus.REFUNDED;
                }
                else
                {
                    refundResponseDto.DependentPaymentTxnOrderResponseCode = ResponseCodeConstants.FAILURE;
                    refundResponseDto.DependentPaymentTxnOrderStatusCode = EnumOrderStatus.PENDING;
                }
            }
            else
            {
                refundResponseDto.DependentPaymentTxnOrderResponseCode = ResponseCodeConstants.FAILURE;
                refundResponseDto.DependentPaymentTxnOrderStatusCode = EnumOrderStatus.PENDING;


            }
            return refundResponseDto;
        }

        public  async Task<InquiryResponseDto> DoInquiryOfRefund(OrderDetailsDto parentOrderDetailsDto, GatewayDto gatewayDto, MerchantGatewayConfigurationMappingDto merchantGatewayConfigurationMappingDto)
        {
            string razorPaymentId = parentOrderDetailsDto.PaymentDataDto.PaymentIdGeneratedByGateway;
            string inquiryRefundUrl = gatewayDto.InquiryRefundUrl;
            string PaymentURL = inquiryRefundUrl + "/" + razorPaymentId + "/";
            string apiUsername = merchantGatewayConfigurationMappingDto.MerchantIdIssuedByGatewayToMerchant;
            string apiPassword = merchantGatewayConfigurationMappingDto.PasswordIssuedByGatewayToMerchant;

            InquiryResponseDto inquiryResponseDto = new InquiryResponseDto();
            inquiryResponseDto.IsRequestSend = true;
            inquiryResponseDto.RequestSendToGateway = GenericUtility.ConvertObjectToJsonString<string>(PaymentURL);
            RazorPayRefundInquiryResponseDto razorPayRefundInquiryResponseDto = await this.apiClient.DoRefundInquiryOnRazorPay(PaymentURL, apiUsername, apiPassword);
            inquiryResponseDto.IsResponseReceived = true;
            if (razorPayRefundInquiryResponseDto != null)
            {
                inquiryResponseDto.ResponseRecievedFromGateway = GenericUtility.ConvertObjectToJsonString<RazorPayRefundInquiryResponseDto>(razorPayRefundInquiryResponseDto);
                inquiryResponseDto.ResponseCodeRecievedFromGateway = razorPayRefundInquiryResponseDto.Status;
                inquiryResponseDto.DependentPaymentTxnOrderResponseCode = ResponseCodeConstants.SUCCESS;
                inquiryResponseDto.DependentPaymentTxnOrderStatusCode = EnumOrderStatus.INQUIRY_COMPLETED;

                if (!string.IsNullOrEmpty(razorPayRefundInquiryResponseDto.Status) &&
                    razorPayRefundInquiryResponseDto.Status.Equals("processed", System.StringComparison.OrdinalIgnoreCase))
                {
                    inquiryResponseDto.IsParentOrderIdTobeUpdate = true;
                    inquiryResponseDto.ParentOrderIdOrderStatusTobeUpdate = (int)EnumOrderStatus.REFUNDED;
                    inquiryResponseDto.ParentOrderIdReponseCodeTobeUpdate = ResponseCodeConstants.SUCCESS;
                    inquiryResponseDto.ParentOrderIdResponseRecievedFromGatewayToBeUpdate = inquiryResponseDto.ResponseRecievedFromGateway;
                  //  inquiryResponseDto.GatewayPaymentId = razorPayRefundInquiryResponseDto.PaymentId;
                    inquiryResponseDto.GatewayOrderID = null;
                }
                //else
                //{
                //    inquiryResponseDto.DependentPaymentTxnOrderResponseCode = ResponseCodeConstants.FAILURE;
                //    inquiryResponseDto.DependentPaymentTxnOrderStatusCode = EnumOrderStatus.PENDING;
                //}
            }
            else
            {
                inquiryResponseDto.DependentPaymentTxnOrderResponseCode = ResponseCodeConstants.FAILURE;
                inquiryResponseDto.DependentPaymentTxnOrderStatusCode = EnumOrderStatus.GATEWAY_ERROR;


            }
            return inquiryResponseDto;
        }
    }
}
