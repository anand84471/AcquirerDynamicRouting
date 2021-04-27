using Core.Constants;
using Core.Features.ExceptionHandling.Abstract;
using Core.Features.ExceptionHandling.Concrete;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Concrete;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutModule.ApiClients.Abstract;
using ExpressCheckoutModule.ApiClients.Concrete;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using ExpressCheckoutPaymentIntegrationModule.Factory.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;
using PinePGController.ExceptionHandling.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ServiceLayer.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IMerchantService _merchantService;
        private readonly IGatewayService _gateWayService;
        private readonly IPaymentIntegrationModuleFactory _paymentIntegrationModuleFactory;
        private readonly ICommonIntegrationHandlerService commonIntegrationHandlerService;
        private readonly IPinePGApiClient _pinePGApiClient;
        private readonly object countinue;


        public OrderService(IOrderRepo orderRepo, IMerchantService merchantService, IGatewayService gateWayService,
                            IPaymentIntegrationModuleFactory paymentIntegrationModuleFactory, ICommonIntegrationHandlerService commonIntegrationHandlerService,IPinePGApiClient pinePGApiClient, ICustomerSavedCardService customerSavedCardService)
        {
            _orderRepo = orderRepo;
            _merchantService = merchantService;
            _gateWayService = gateWayService;
            _paymentIntegrationModuleFactory = paymentIntegrationModuleFactory;
            this.commonIntegrationHandlerService = commonIntegrationHandlerService;
            _pinePGApiClient = pinePGApiClient;

        }

        public Task<bool> CheckMerchantOrderIdExist(MerchantDto merchantDto)
        {
            throw new NotImplementedException();
        }

        public async Task<long> CreateOrder(OrderDetailsDto orderDetailsDto)
        {
            long AggOrderId = await _orderRepo.CreateOrder(orderDetailsDto);
            if (AggOrderId == 0)
            {
                throw new OrderException(ResponseCodeConstants.UNABLE_TO_GENERATE_ORDER);
            }
            return AggOrderId;
        }
        public async Task<bool> UpdatePurchaseOrder(OrderDetailsDto orderDetailsDto)
        {
            bool status = await _orderRepo.UpdatePurchaseOrder(orderDetailsDto);
           
            return status;
        }
        public async Task<long> InsertPaymentDetails(OrderPaymentDetailsDto orderPaymentDetailsDto)
        {
            long aggPaymentId = await _orderRepo.InsertPaymentDetails(orderPaymentDetailsDto);
            if (aggPaymentId == 0)
            {
                throw new OrderException(ResponseCodeConstants.UNABLE_TO_GENERATE_ORDER);
            }
            return aggPaymentId;
        }

        public async Task<OrderDetailsDto> GetOrderDetails(long AggOrderId)
        {
            return await _orderRepo.GetOrderDetails(AggOrderId);
        }

        public async Task<long> CreateOrderForInquiry(OrderDetailsDto parentOrderDetailsDto)
        {
            OrderDetailsDto orderInquiryTxnDto = new OrderDetailsDto();
            orderInquiryTxnDto.MerchantDto = new MerchantDto();
            orderInquiryTxnDto.OrderTxnInfoDto = new OrderTxnInfoDto();
            orderInquiryTxnDto.MerchantDto.MerchantId = parentOrderDetailsDto.MerchantDto.MerchantId;
            orderInquiryTxnDto.MerchantDto.MerchantOrderID = parentOrderDetailsDto.MerchantDto.MerchantOrderID;
            orderInquiryTxnDto.OrderTxnInfoDto.TxnTypeCode = EnumTxnType.INQUIRY;
            orderInquiryTxnDto.OrderTxnInfoDto.ParentOrderId = parentOrderDetailsDto.OrderTxnInfoDto.AggOrderId;
            long inquiryOrderId = await this.CreateOrder(orderInquiryTxnDto);
            return inquiryOrderId;
        }
        public async Task<long> CreatePaymentForOrderInquiry(OrderDetailsDto parentOrderDetailsDto, long dependentInquiryOrderId)
        {
            OrderPaymentDetailsDto orderPaymentDetailsDto = new OrderPaymentDetailsDto();
            orderPaymentDetailsDto.AggOrderId = dependentInquiryOrderId;
            orderPaymentDetailsDto.GatewayId = parentOrderDetailsDto.PaymentDataDto.GatewayId;
            orderPaymentDetailsDto.RequestTypeCode = EnumRequestType.INQUIRY;
            long dependentPaymentId = await this.InsertPaymentDetails(orderPaymentDetailsDto);
            return dependentPaymentId;
        }

        public async Task<OrderDetailsDto> DoOrderInquiry(OrderDetailsDto parentOrderDetailsDto)
        {
            bool isOrderStateFetchFromDB = false;
            if (parentOrderDetailsDto.OrderTxnInfoDto.OrderStatusCode != EnumOrderStatus.ORDER_CREATED)
            {

                long dependentOrderId = await this.CreateOrderForInquiry(parentOrderDetailsDto);
                if (dependentOrderId <= 0)
                {
                    throw new DBException(ResponseCodeConstants.FAILURE);
                }
                long dependentPaymentId = await this.CreatePaymentForOrderInquiry(parentOrderDetailsDto, dependentOrderId);
                if (dependentPaymentId <= 0)
                {
                    throw new DBException(ResponseCodeConstants.FAILURE);
                }
                int gatewayId = parentOrderDetailsDto.PaymentDataDto.GatewayId;
                InquiryResponseDto inquiryResponseDto = null;
                OrderStepDto dependentOrderStepDto = new OrderStepDto();
                bool parentDetailUpdateStatus = false;
                if (parentOrderDetailsDto.OrderTxnInfoDto.OrderStatusCode == EnumOrderStatus.AUTHORIZING || parentOrderDetailsDto.OrderTxnInfoDto.OrderStatusCode == EnumOrderStatus.PENDING)
                {
                    isOrderStateFetchFromDB = true;

                    Task<GatewayDto> taskGatewayDto = _gateWayService.GetGatewayDetails(gatewayId);
                    Task<MerchantGatewayConfigurationMappingDto> taskMerchantGatewayConfigurationMappingDto = _merchantService.GetMerchantPaymentGatewayConfigurationDetails(parentOrderDetailsDto.MerchantDto.MerchantId, gatewayId);

                    GatewayDto gatewayDto = await taskGatewayDto;
                    MerchantGatewayConfigurationMappingDto merchantGatewayConfigurationMappingDto = await taskMerchantGatewayConfigurationMappingDto;

                    if (gatewayDto != null && merchantGatewayConfigurationMappingDto != null)
                    {
                        //Get Gateway handler service
                        //Do Inquiry for gateway
                        IGatewayIntegrationHandlerService gatewayIntegrationHandlerService = _paymentIntegrationModuleFactory.GetPaymentGatewayHandlerService((EnumGateway)gatewayId);
                        if (parentOrderDetailsDto.OrderTxnInfoDto.TxnTypeCode == EnumTxnType.PURCHASE)
                        {
                            inquiryResponseDto = await gatewayIntegrationHandlerService.DoInquiryOfPurchase(parentOrderDetailsDto, gatewayDto, merchantGatewayConfigurationMappingDto);
                        }
                        else
                        {
                            inquiryResponseDto = await gatewayIntegrationHandlerService.DoInquiryOfRefund(parentOrderDetailsDto, gatewayDto, merchantGatewayConfigurationMappingDto);

                        }


                        if (inquiryResponseDto != null)
                        {
                            if (inquiryResponseDto.IsParentOrderIdTobeUpdate)
                            {
                                /*update parent  txn order and payment if payment is succesful
                                 * 1.Update payment details for parent order id
                                 * 2.update order status of parent order
                                 * 
                                 */
                                #region update parent order details

                                UpdateOrderDetailsDto parentOrderUpdateOrderDetailsDto = new UpdateOrderDetailsDto();
                                UpdateOrderDetailsDto purchaseTxnRefundParentOrderUpdateOrderDetailsDto = new UpdateOrderDetailsDto();
                                if (parentOrderDetailsDto.OrderTxnInfoDto.TxnTypeCode == EnumTxnType.REFUND)
                                {
                                    // UpdateOrderDetailsDto purchaseTxnRefundParentOrderUpdateOrderDetailsDto = new UpdateOrderDetailsDto();
                                    if (inquiryResponseDto.ParentOrderIdOrderStatusTobeUpdate == (int)EnumOrderStatus.REFUNDED)
                                    {
                                        purchaseTxnRefundParentOrderUpdateOrderDetailsDto = new UpdateOrderDetailsDto();
                                        purchaseTxnRefundParentOrderUpdateOrderDetailsDto.AggOrderId = parentOrderDetailsDto.OrderTxnInfoDto.ParentOrderId;
                                        purchaseTxnRefundParentOrderUpdateOrderDetailsDto.RefundAmount = parentOrderDetailsDto.OrderTxnInfoDto.Amount;
                                        purchaseTxnRefundParentOrderUpdateOrderDetailsDto.OrderStatus = inquiryResponseDto.ParentOrderIdOrderStatusTobeUpdate;
                                        purchaseTxnRefundParentOrderUpdateOrderDetailsDto.OrderResponseCode = inquiryResponseDto.ParentOrderIdReponseCodeTobeUpdate;
                                        // await _orderRepo.UpdateOrderDetails(purchaseTxnRefundParentOrderUpdateOrderDetailsDto);
                                    }

                                    //UpdateOrderDetailsDto parentOrderUpdateOrderDetailsDto = new UpdateOrderDetailsDto();
                                    parentOrderUpdateOrderDetailsDto.AggOrderId = parentOrderDetailsDto.OrderTxnInfoDto.AggOrderId;
                                    parentOrderUpdateOrderDetailsDto.OrderStatus = inquiryResponseDto.ParentOrderIdOrderStatusTobeUpdate;
                                    parentOrderUpdateOrderDetailsDto.OrderResponseCode = inquiryResponseDto.ParentOrderIdReponseCodeTobeUpdate;
                                    parentOrderUpdateOrderDetailsDto.RefundAmount = parentOrderDetailsDto.OrderTxnInfoDto.Amount;
                                    // await _orderRepo.UpdateOrderDetails(parentOrderUpdateOrderDetailsDto);

                                    //update refund parent txn purchase


                                }
                                else
                                {
                                    //  UpdateOrderDetailsDto parentOrderUpdateOrderDetailsDto = new UpdateOrderDetailsDto();
                                    parentOrderUpdateOrderDetailsDto.AggOrderId = parentOrderDetailsDto.OrderTxnInfoDto.AggOrderId;
                                    parentOrderUpdateOrderDetailsDto.OrderStatus = inquiryResponseDto.ParentOrderIdOrderStatusTobeUpdate;
                                    parentOrderUpdateOrderDetailsDto.OrderResponseCode = inquiryResponseDto.ParentOrderIdReponseCodeTobeUpdate;
                                    // await _orderRepo.UpdateOrderDetails(parentOrderUpdateOrderDetailsDto);
                                }

                                OrderStepDto parentOrderStepDto = new OrderStepDto();
                                parentOrderStepDto.PaymentId = parentOrderDetailsDto.PaymentDataDto.PaymentId;
                                parentOrderStepDto.RequestType = parentOrderDetailsDto.PaymentDataDto.RequestType;
                                parentOrderStepDto.ResponseReceivedFromGateway = inquiryResponseDto.ResponseRecievedFromGateway;
                                parentOrderStepDto.ResponseCodeReceivedFromGateway = inquiryResponseDto.ResponseCodeRecievedFromGateway;
                                //await _orderRepo.UpdateOrderPaymentStepDetails(parentOrderStepDto);

                                OrderPaymentDetailsDto parentOrderPaymentDetailsDto = new OrderPaymentDetailsDto();
                                parentOrderPaymentDetailsDto.AggOrderId = parentOrderDetailsDto.OrderTxnInfoDto.AggOrderId;
                                parentOrderPaymentDetailsDto.AggPaymentId = parentOrderDetailsDto.PaymentDataDto.PaymentId;
                                parentOrderPaymentDetailsDto.PaymentStatusCode = (EnumOrderStatus)inquiryResponseDto.ParentOrderIdOrderStatusTobeUpdate;
                                parentOrderPaymentDetailsDto.PaymentResponseCode = inquiryResponseDto.ParentOrderIdReponseCodeTobeUpdate;
                                parentOrderPaymentDetailsDto.GatewayOrderID = inquiryResponseDto.GatewayOrderID;
                                parentOrderPaymentDetailsDto.GatewayPaymentId = inquiryResponseDto.GatewayPaymentId;
                                // await _orderRepo.UpdateOrderPaymentDetails(parentOrderPaymentDetailsDto);

                                parentDetailUpdateStatus = await _orderRepo.UpdateParentTransactionOrderDetails
                                    (parentOrderPaymentDetailsDto, parentOrderStepDto, parentOrderUpdateOrderDetailsDto, purchaseTxnRefundParentOrderUpdateOrderDetailsDto);




                                #endregion


                                #region dependent order details

                                dependentOrderStepDto.PaymentId = dependentPaymentId;
                                dependentOrderStepDto.RequestTypeCode = EnumRequestType.INQUIRY;
                                dependentOrderStepDto.RequestSendToGateway = inquiryResponseDto.RequestSendToGateway;
                                dependentOrderStepDto.ResponseReceivedFromGateway = inquiryResponseDto.ResponseRecievedFromGateway;
                                dependentOrderStepDto.ResponseCodeReceivedFromGateway = inquiryResponseDto.ResponseCodeRecievedFromGateway;
                                //await _orderRepo.UpdateOrderPaymentStepDetails(dependentOrderStepDto);
                                #endregion

                            }


                        }

                    }
                }

                EnumOrderStatus paymentStatusCode = EnumOrderStatus.INQUIRY_COMPLETED;
                int paymentResponseCode = ResponseCodeConstants.SUCCESS;


                if (isOrderStateFetchFromDB)
                {
                    if (inquiryResponseDto != null)
                    {
                        paymentStatusCode = inquiryResponseDto.DependentPaymentTxnOrderStatusCode;
                        paymentResponseCode = inquiryResponseDto.DependentPaymentTxnOrderResponseCode;

                    }

                }



                #region dependent order details




                OrderPaymentDetailsDto dependentOrderDetailsDto = new OrderPaymentDetailsDto();
                dependentOrderDetailsDto.AggOrderId = dependentOrderId;
                dependentOrderDetailsDto.AggPaymentId = dependentPaymentId;
                dependentOrderDetailsDto.PaymentStatusCode = paymentStatusCode;
                dependentOrderDetailsDto.PaymentResponseCode = paymentResponseCode;
                //  await _orderRepo.UpdateOrderPaymentDetails(dependentOrderDetailsDto);

                UpdateOrderDetailsDto dependentOrderUpdateOrderDetailsDto = new UpdateOrderDetailsDto();
                dependentOrderUpdateOrderDetailsDto.AggOrderId = dependentOrderId;
                dependentOrderUpdateOrderDetailsDto.PaymentId = dependentPaymentId;
                dependentOrderUpdateOrderDetailsDto.OrderStatusCode = paymentStatusCode;
                dependentOrderUpdateOrderDetailsDto.OrderResponseCode = paymentResponseCode;
                dependentOrderUpdateOrderDetailsDto.GatewayId = gatewayId;
                dependentOrderUpdateOrderDetailsDto.RequestTypeCode = EnumRequestType.INQUIRY;
                //await _orderRepo.UpdateOrderDetails(dependentOrderUpdateOrderDetailsDto);
                #endregion


                if (inquiryResponseDto != null && inquiryResponseDto.IsParentOrderIdTobeUpdate && !parentDetailUpdateStatus)
                {
                    dependentOrderDetailsDto.PaymentStatusCode = EnumOrderStatus.INQUIRY_COMPLETED;
                    dependentOrderDetailsDto.PaymentResponseCode = ResponseCodeConstants.DB_FAILURE;
                    dependentOrderUpdateOrderDetailsDto.OrderStatusCode = EnumOrderStatus.INQUIRY_COMPLETED;
                    dependentOrderUpdateOrderDetailsDto.OrderResponseCode = ResponseCodeConstants.DB_FAILURE;
                }

                await _orderRepo.UpdateDependentOrderDetails(dependentOrderDetailsDto, dependentOrderStepDto,
                    dependentOrderUpdateOrderDetailsDto);
                //update inquiry status txn
                if (isOrderStateFetchFromDB)
                {
                    parentOrderDetailsDto = await _orderRepo.GetOrderDetails(parentOrderDetailsDto.OrderTxnInfoDto.AggOrderId);
                }
                return parentOrderDetailsDto;

            }

            return parentOrderDetailsDto;
        }

        public async Task<string> DoPayment(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetails)
        {
            var response = "";
            DynamicRoutingGatewayResponse dynamicRoutingGatewayResponse;
            List<string> list=null;
            List<int> EnableGateway=null;
            dynamicRoutingGatewayResponse = await _gateWayService.GatewayOrderDetails(orderDetails.OrderTxnInfoDto.AggOrderId);
            EnableGateway = await _gateWayService.EnablePaymentModeList(orderDetails.MerchantDto.MerchantId, Convert.ToInt32(doPaymentRequest.PaymentDetailsRequest.PaymentMode));
            if (dynamicRoutingGatewayResponse != null)
            {
                // list = new List<string>(dynamicRoutingGatewayResponse.Priorties);
                list = null;
            }
            if (dynamicRoutingGatewayResponse == null)
            {
                if (EnableGateway == null)
                {
                    throw new DBException(ResponseCodeConstants.NOT_ENABLE_PAYMENT_MODE_FOR_MERCHANT);
                }
                list = EnableGateway.ConvertAll<string>(x => x.ToString());
            }
            foreach (string listGateway in list)
            {
                try
                {
                    doPaymentRequest.PaymentDetailsRequest.PrefferedGatewayCode = (EnumGateway)Enum.Parse(typeof(EnumGateway), listGateway);
                   //check this gateway existing list or not for merchant enable
                    int checkGateway = Convert.ToInt32(doPaymentRequest.PaymentDetailsRequest.PrefferedGatewayCode);
                    if (dynamicRoutingGatewayResponse != null)
                    {
                        if (!EnableGateway.Contains(checkGateway))
                        {
                            continue;
                        }
                    }
                    //var gateWays = commonIntegrationHandlerService.GetGatewaysAccordingToDynamicRouting(doPaymentRequest.MerchantDetailsRequest.MerchantId);
                    var gatewayDetails = await this._gateWayService.GetGatewayDetails(Convert.ToInt32(doPaymentRequest.PaymentDetailsRequest.PrefferedGatewayCode));
                    if (gatewayDetails == null)
                    {
                        throw new OrderException(ResponseCodeConstants.GATEWAY_DETAILS_NOT_PRESENT);
                    }
                    orderDetails.GatewayDto = gatewayDetails;

                    var merchantGatewayConfig = await _merchantService.GetMerchantPaymentGatewayConfigurationDetails(doPaymentRequest.MerchantDetailsRequest.MerchantId, Convert.ToInt32(doPaymentRequest.PaymentDetailsRequest.PrefferedGatewayCode));
                    if (merchantGatewayConfig == null)
                    {
                        throw new OrderException(ResponseCodeConstants.MERCHANT_CONFIG_NOT_PRESENT);
                    }
                    orderDetails.MerchantGatewayConfigurationMappingDto = merchantGatewayConfig;

                    var aggPaymentId = await _orderRepo.InsertOrderPaymentDetailsAndStepDetails(doPaymentRequest, orderDetails);
                    if (aggPaymentId <= 0)
                    {
                        throw new DBException(ResponseCodeConstants.FAILURE);
                    }
                    orderDetails.PaymentDataDto = new PaymentDataDto();
                    orderDetails.PaymentDataDto.PaymentId = aggPaymentId;

                    var paymentGatewayHandlerService = _paymentIntegrationModuleFactory.GetPaymentGatewayHandlerService((EnumGateway)Enum.Parse(typeof(EnumGateway), listGateway));
                    response = await paymentGatewayHandlerService.DoPurchase(doPaymentRequest, orderDetails);
                    return response;
                }
                catch (MasterException ex)
                {
                    if (ex.ResponseCode == ResponseCodeConstants.ORDER_CREATION_FAILED_AT_AGGREGATOR)
                    {
                        continue;
                    }
                }
            }
                return response;
            
        }

        public async Task<OrderDetailsDto> DoOrderRefund(OrderDetailsDto parentOrderDetailsDto, OrderDetailsDto refundRequestDto)
        {
            long AggOrderIdToBeRefunded = refundRequestDto.OrderTxnInfoDto.AggOrderId;
            long refundAmount = refundRequestDto.OrderTxnInfoDto.Amount;
            int merchantId = refundRequestDto.MerchantDto.MerchantId;
            string merchantOrderId = refundRequestDto.MerchantDto.MerchantOrderID;
            long dependentOrderId = await this.CreateOrderForRefund(refundRequestDto);
            if (dependentOrderId <= 0)
            {
                throw new DBException(ResponseCodeConstants.FAILURE);
            }
            long dependentPaymentId = await this.CreatePaymentForOrderRefund(parentOrderDetailsDto, dependentOrderId);
            if (dependentPaymentId <= 0)
            {
                throw new DBException(ResponseCodeConstants.FAILURE);
            }

            int gatewayId = parentOrderDetailsDto.PaymentDataDto.GatewayId;


            Task<GatewayDto> taskGatewayDto = _gateWayService.GetGatewayDetails(gatewayId);
            Task<MerchantGatewayConfigurationMappingDto> taskMerchantGatewayConfigurationMappingDto = _merchantService.GetMerchantPaymentGatewayConfigurationDetails(merchantId, gatewayId);

            GatewayDto gatewayDto = await taskGatewayDto;
            MerchantGatewayConfigurationMappingDto merchantGatewayConfigurationMappingDto = await taskMerchantGatewayConfigurationMappingDto;

            if (gatewayDto != null && merchantGatewayConfigurationMappingDto != null)
            {
                //Get Gateway handler service
                //Do Inquiry for gateway
                IGatewayIntegrationHandlerService gatewayIntegrationHandlerService = _paymentIntegrationModuleFactory.GetPaymentGatewayHandlerService((EnumGateway)gatewayId);
                RefundResponseDto refundResponseDto = await gatewayIntegrationHandlerService.DoRefund(parentOrderDetailsDto, refundAmount, gatewayDto, merchantGatewayConfigurationMappingDto);
                bool parentDetailUpdateStatus = false;
                if (refundResponseDto != null)
                {
                    if (refundResponseDto.IsParentOrderIdTobeUpdate)
                    {
                        /*update parent  txn order and payment if payment is succesful
                            * 1.Update payment details for parent ordre id
                            * 2.update order status of parent order
                            * 
                            */
                        UpdateOrderDetailsDto parentOrderUpdateOrderDetailsDto = new UpdateOrderDetailsDto();
                        parentOrderUpdateOrderDetailsDto.AggOrderId = AggOrderIdToBeRefunded;
                        parentOrderUpdateOrderDetailsDto.RefundAmount = refundAmount;
                        parentOrderUpdateOrderDetailsDto.OrderStatus = refundResponseDto.ParentOrderIdOrderStatusTobeUpdate;
                        parentDetailUpdateStatus = await _orderRepo.UpdateOrderDetails(parentOrderUpdateOrderDetailsDto);

                    }


                    OrderStepDto dependentOrderStepDto = new OrderStepDto();
                    dependentOrderStepDto.PaymentId = dependentPaymentId;
                    dependentOrderStepDto.RequestTypeCode = EnumRequestType.REFUND;
                    dependentOrderStepDto.RequestSendToGateway = refundResponseDto.RequestSendToGateway;
                    dependentOrderStepDto.ResponseReceivedFromGateway = refundResponseDto.ResponseRecievedFromGateway;
                    dependentOrderStepDto.ResponseCodeReceivedFromGateway = refundResponseDto.ResponseCodeRecievedFromateway;
                    // await _orderRepo.UpdateOrderPaymentStepDetails(dependentOrderStepDto);

                    OrderPaymentDetailsDto dependentOrderUpdatePaymentDetailsDto = new OrderPaymentDetailsDto();
                    dependentOrderUpdatePaymentDetailsDto.AggOrderId = dependentOrderId;
                    dependentOrderUpdatePaymentDetailsDto.AggPaymentId = dependentPaymentId;
                    dependentOrderUpdatePaymentDetailsDto.PaymentStatusCode = refundResponseDto.DependentPaymentTxnOrderStatusCode;
                    dependentOrderUpdatePaymentDetailsDto.PaymentResponseCode = refundResponseDto.DependentPaymentTxnOrderResponseCode;
                    dependentOrderUpdatePaymentDetailsDto.GatewayOrderID = refundResponseDto.GatewayOrderID;
                    dependentOrderUpdatePaymentDetailsDto.GatewayPaymentId = refundResponseDto.GatewayPaymentId;
                    //await _orderRepo.UpdateOrderPaymentDetails(dependentOrderUpdatePaymentDetailsDto);

                    UpdateOrderDetailsDto dependentOrderUpdateOrderDetailsDto = new UpdateOrderDetailsDto();
                    dependentOrderUpdateOrderDetailsDto.AggOrderId = dependentOrderId;
                    dependentOrderUpdateOrderDetailsDto.PaymentId = dependentPaymentId;
                    dependentOrderUpdateOrderDetailsDto.PaymentId = dependentPaymentId;
                    dependentOrderUpdateOrderDetailsDto.OrderStatusCode = refundResponseDto.DependentPaymentTxnOrderStatusCode;
                    dependentOrderUpdateOrderDetailsDto.OrderResponseCode = refundResponseDto.DependentPaymentTxnOrderResponseCode;
                    dependentOrderUpdateOrderDetailsDto.GatewayId = gatewayId;
                    dependentOrderUpdateOrderDetailsDto.RequestTypeCode = EnumRequestType.REFUND;
                    // await _orderRepo.UpdateOrderDetails(dependentOrderUpdateOrderDetailsDto);

                    if (refundResponseDto != null && refundResponseDto.IsParentOrderIdTobeUpdate && !parentDetailUpdateStatus)
                    {
                        dependentOrderUpdatePaymentDetailsDto.PaymentStatusCode = EnumOrderStatus.PENDING;
                        dependentOrderUpdatePaymentDetailsDto.PaymentResponseCode = ResponseCodeConstants.FAILURE;
                        dependentOrderUpdateOrderDetailsDto.OrderStatusCode = EnumOrderStatus.PENDING;
                        dependentOrderUpdateOrderDetailsDto.OrderResponseCode = ResponseCodeConstants.FAILURE;


                    }

                    await _orderRepo.UpdateDependentOrderDetails(dependentOrderUpdatePaymentDetailsDto, dependentOrderStepDto,
                        dependentOrderUpdateOrderDetailsDto);


                }

            }

            return await _orderRepo.GetOrderDetails(dependentOrderId);




        }
        private async Task<long> CreatePaymentForOrderRefund(OrderDetailsDto parentOrderDetailsDto, long dependentRefundOrderId)
        {
            OrderPaymentDetailsDto dependentOrderPaymentDetailsDto = new OrderPaymentDetailsDto();
            dependentOrderPaymentDetailsDto.AggOrderId = dependentRefundOrderId;
            dependentOrderPaymentDetailsDto.GatewayId = parentOrderDetailsDto.PaymentDataDto.GatewayId;
            dependentOrderPaymentDetailsDto.RequestTypeCode = EnumRequestType.REFUND;
            return await this.InsertPaymentDetails(dependentOrderPaymentDetailsDto);
        }

        private async Task<long> CreateOrderForRefund(OrderDetailsDto refundOrderRequest)
        {
            OrderDetailsDto dependentOrderRefundTxnDto = new OrderDetailsDto();
            dependentOrderRefundTxnDto.MerchantDto = new MerchantDto();
            dependentOrderRefundTxnDto.OrderTxnInfoDto = new OrderTxnInfoDto();
            dependentOrderRefundTxnDto.MerchantDto.MerchantId = refundOrderRequest.MerchantDto.MerchantId;
            dependentOrderRefundTxnDto.MerchantDto.MerchantOrderID = refundOrderRequest.MerchantDto.MerchantOrderID;
            dependentOrderRefundTxnDto.OrderTxnInfoDto.TxnTypeCode = EnumTxnType.REFUND;
            dependentOrderRefundTxnDto.OrderTxnInfoDto.Amount = refundOrderRequest.OrderTxnInfoDto.Amount;
            dependentOrderRefundTxnDto.OrderTxnInfoDto.ParentOrderId = refundOrderRequest.OrderTxnInfoDto.AggOrderId;
            dependentOrderRefundTxnDto.OrderTxnInfoDto.CurrencyCode = refundOrderRequest.OrderTxnInfoDto.CurrencyCode;

            return await this.CreateOrder(dependentOrderRefundTxnDto);
        }

        public Task<OrderPaymentDetailsDto> GetAggregatorOrderDetailsByGatewayOrderIdAsync(EnumGateway enumGateway, string gatewayOrderId)
        {
            return this._orderRepo.GetAggregatorOrderDetailsByGatewayOrderIdAsync(enumGateway, gatewayOrderId);
        }



        //public async Task<OrderDetailsDto> GetOrderDetails(int merchantId, string merchantOrderID)
        //{
        //    OrderDetailsDto orderDetailsDtoDB=await  _orderRepo.GetOrderDetails(merchantId, merchantOrderID);
        //}
    }
}