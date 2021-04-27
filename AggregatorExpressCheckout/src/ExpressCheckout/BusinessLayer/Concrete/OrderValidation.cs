using AutoMapper;
using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using Core.Utilities;
using Core.Validation;
using ExpressCheckout.BusinessLayer.Abstract;
using ExpressCheckout.Validators;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Concrete;
using ExpressCheckoutModule.Cache.Abstract;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressCheckout.BusinessLayer.Concrete
{
    public class OrderValidation : IOrderValidation
    {
        private readonly IMapper _mapper;
        private ICustomerSavedCardService _CustomerSavedCardService;
        private IMerchantService _MerchantService;
        private ICustomerService _CustomerService;
        private ICustomerSavedCardValidation _customerSavedCardValidation;
        private IOrderService _orderService;
        private IExpressCheckoutCache _coreCache;
        private IGlobalBinDataService _globalBinDataService;


        public OrderValidation(IMapper mapper, IOrderService orderService, ICustomerSavedCardService customerSavedCardService,
                               IMerchantService MerchantService, ICustomerService CustomerService, IExpressCheckoutCache coreCache, IGlobalBinDataService globalBinDataService, ICustomerSavedCardValidation customerSavedCardValidation)
        {
            _mapper = mapper;
            _CustomerSavedCardService = customerSavedCardService;
            _MerchantService = MerchantService;
            _CustomerService = CustomerService;
            _orderService = orderService;
            _coreCache = coreCache;
            _globalBinDataService = globalBinDataService;
            _customerSavedCardValidation = customerSavedCardValidation;
        }

        public async Task<long> CreatePurchaseOrder(IHeaderDictionary headers, AcceptOrderDetailsRequest acceptOrderDetailsRequest)
        {
            if (acceptOrderDetailsRequest == null || string.IsNullOrEmpty(acceptOrderDetailsRequest.Request))
            {
                throw new OrderException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }

            string jsonCustomHeaderString = GenericUtility.GetJsonStringFromHttpRequestHeader(headers);
            if (String.IsNullOrEmpty(jsonCustomHeaderString))
            {
                throw new OrderException(ResponseCodeConstants.ORDER_DATA_IS_NOT_VALID);
            }
            acceptOrderDetailsRequest.CustomHeader = GenericUtility.GetObjectFromJsonString<CustomHeader>(jsonCustomHeaderString);
            string jsonRequestString = GenericUtility.GetJsonStringFromBase64EncodeStriing(acceptOrderDetailsRequest.Request);
            OrderDetailsRequest orderDetailsRequest = GenericUtility.GetObjectFromJsonString<OrderDetailsRequest>(jsonRequestString);

            if (orderDetailsRequest == null || GenericUtility.ValidateForXSSAttackAttempt(orderDetailsRequest))
            {
                throw new OrderException(ResponseCodeConstants.ORDER_DATA_IS_NOT_VALID);
            }

            OrderDetailsDto orderDetailsDto = _mapper.Map<OrderDetailsDto>(orderDetailsRequest);
            _mapper.Map<AcceptOrderDetailsRequest, OrderDetailsDto>(acceptOrderDetailsRequest, orderDetailsDto);

            ValidationHander<OrderValidator, OrderDetailsDto>.DoValidate(orderDetailsDto, ConstantRuleSetName.PURCHASE_TXN_VALIDATION);
            MerchantDto merchantDto = await _MerchantService.GetMerchantData(orderDetailsDto.MerchantDto.MerchantId);
            if (merchantDto == null)
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
            }

            //if(!GenericUtility.ValidateSecureIncomingRequest(acceptOrderDetailsRequest.Request, acceptOrderDetailsRequest.CustomHeader.XVerify, merchantDto.SecureSeret))
            //{
            //    throw new OrderException(ResponseCodeConstants.INAVLID_SECURE_SHA);
            //}

            if (!_MerchantService.IsMerchantValid(merchantDto, orderDetailsDto.MerchantDto.MerchantAccessCode))
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
            }

            if (await _MerchantService.IsDuplicateMerchantIDAndMerchantOrderId(orderDetailsDto.MerchantDto.MerchantId, orderDetailsDto.MerchantDto.MerchantOrderID))
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_ORDER_ID_ALREADY_EXISTS);
            }

            orderDetailsDto.JsonRequestString = jsonRequestString;
            orderDetailsDto.OrderTxnInfoDto.TxnTypeCode = EnumTxnType.PURCHASE;
            orderDetailsDto.OrderTxnInfoDto.OrderStatusCode = EnumOrderStatus.ORDER_CREATED;
            orderDetailsDto.OrderTxnInfoDto.OrderResponseCode = ResponseCodeConstants.SUCCESS;
            long AggOrderId = await _orderService.CreateOrder(orderDetailsDto);
            orderDetailsDto.OrderTxnInfoDto.AggOrderId = AggOrderId;
            _coreCache.AddOrderDetailsRequest(AggOrderId, orderDetailsDto);
            return AggOrderId;
        }

        public async Task<bool> UpdatePurchaseOrdrer(IHeaderDictionary headers, AcceptOrderDetailsRequest acceptOrderDetailsRequest)
        {
            bool status = false;
            if (acceptOrderDetailsRequest == null || string.IsNullOrEmpty(acceptOrderDetailsRequest.Request))
            {
                throw new OrderException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }

            string jsonCustomHeaderString = GenericUtility.GetJsonStringFromHttpRequestHeader(headers);
            if (String.IsNullOrEmpty(jsonCustomHeaderString))
            {
                throw new OrderException(ResponseCodeConstants.ORDER_DATA_IS_NOT_VALID);
            }
            acceptOrderDetailsRequest.CustomHeader = GenericUtility.GetObjectFromJsonString<CustomHeader>(jsonCustomHeaderString);
            string jsonRequestString = GenericUtility.GetJsonStringFromBase64EncodeStriing(acceptOrderDetailsRequest.Request);
            OrderDetailsRequest orderDetailsRequest = GenericUtility.GetObjectFromJsonString<OrderDetailsRequest>(jsonRequestString);
            if (orderDetailsRequest == null || GenericUtility.ValidateForXSSAttackAttempt(orderDetailsRequest))
            {
                throw new OrderException(ResponseCodeConstants.ORDER_DATA_IS_NOT_VALID);
            }
            OrderDetailsDto updateOrderDto = _mapper.Map<OrderDetailsDto>(orderDetailsRequest);
            _mapper.Map<AcceptOrderDetailsRequest, OrderDetailsDto>(acceptOrderDetailsRequest, updateOrderDto);
            ValidationHander<OrderValidator, OrderDetailsDto>.DoValidate(updateOrderDto, ConstantRuleSetName.PURCHASE_TXN_VALIDATION);
            MerchantDto merchantDto = await _MerchantService.GetMerchantData(updateOrderDto.MerchantDto.MerchantId);
            if (merchantDto == null)
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
            }
            if (!_MerchantService.IsMerchantValid(merchantDto, updateOrderDto.MerchantDto.MerchantAccessCode))
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
            }
            OrderDetailsDto orderDetailsDtoFromDB = await _orderService.GetOrderDetails(Convert.ToInt64(updateOrderDto.MerchantDto.MerchantOrderID));
            if (orderDetailsDtoFromDB == null)
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_ORDER_ID_ALREADY_EXISTS);
            }
            if (orderDetailsDtoFromDB.OrderTxnInfoDto.OrderStatusCode == EnumOrderStatus.ORDER_CREATED)
            {

                updateOrderDto.JsonRequestString = jsonRequestString;
                status = await _orderService.UpdatePurchaseOrder(updateOrderDto);
            }
            else
            {
                throw new OrderException(ResponseCodeConstants.UPDATION_FAILD_STATUS_NOT_CREATED_STATE);
            }

            return status;
        }

        public async Task<string> DoPayment(long orderId, DoPaymentRequest doPaymentRequest)
        {
            if (orderId <= default(long) || doPaymentRequest == null)
            {
                throw new OrderException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }
            var orderDetails = _coreCache.GetOrderDetailsRequest(orderId);

            if (doPaymentRequest.MerchantDetailsRequest != null && orderDetails.MerchantDto.MerchantId != doPaymentRequest.MerchantDetailsRequest.MerchantId)
            {
                throw new OrderException(ResponseCodeConstants.INVALID_MERCHANT_ID);
            }
            
            doPaymentRequest= await CheckSavedCardOrNotAsync(orderDetails, doPaymentRequest);
            await _globalBinDataService.InsertGlobalBindata(doPaymentRequest, orderId);
            return await _orderService.DoPayment(doPaymentRequest, orderDetails);
        }

        public async Task<OrderDetailsResponseSentToMerchant> DoInquiry(IHeaderDictionary headers, AcceptOrderDetailsRequest acceptOrderDetailsRequest)
        {

            if (acceptOrderDetailsRequest == null || string.IsNullOrEmpty(acceptOrderDetailsRequest.Request))
            {
                throw new OrderException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }

            string jsonCustomHeaderString = GenericUtility.GetJsonStringFromHttpRequestHeader(headers);
            if (String.IsNullOrEmpty(jsonCustomHeaderString))
            {
                throw new OrderException(ResponseCodeConstants.ORDER_DATA_IS_NOT_VALID);
            }
            acceptOrderDetailsRequest.CustomHeader = GenericUtility.GetObjectFromJsonString<CustomHeader>(jsonCustomHeaderString);
            string jsonRequestString = GenericUtility.GetJsonStringFromBase64EncodeStriing(acceptOrderDetailsRequest.Request);
            DependentOrderRequest inquiryRequest = GenericUtility.GetObjectFromJsonString<DependentOrderRequest>(jsonRequestString);

            if (inquiryRequest == null || GenericUtility.ValidateForXSSAttackAttempt(inquiryRequest))
            {
                throw new OrderException(ResponseCodeConstants.ORDER_DATA_IS_NOT_VALID);
            }

            OrderDetailsResponseSentToMerchant orderDetailsResponseSentToMerchant = null;


            OrderDetailsDto inquiryRequestDto = _mapper.Map<OrderDetailsDto>(inquiryRequest);
            if (inquiryRequestDto.MerchantDto == null || inquiryRequestDto.OrderTxnInfoDto == null ||
              inquiryRequestDto.MerchantDto.MerchantId <= default(int) || String.IsNullOrEmpty(inquiryRequestDto.MerchantDto.MerchantAccessCode) ||
              inquiryRequestDto.OrderTxnInfoDto.AggOrderId <= default(long))
            {
                throw new OrderException(ResponseCodeConstants.ORDER_DATA_IS_NOT_VALID);
            }
            long parentAggOrderId = inquiryRequestDto.OrderTxnInfoDto.AggOrderId;
            int merchantId = inquiryRequestDto.MerchantDto.MerchantId;
            string merchantAccessCode = inquiryRequestDto.MerchantDto.MerchantAccessCode;

            MerchantDto merchantDto = await _MerchantService.GetMerchantData(merchantId);
            if (merchantDto == null)
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
            }

            if (!_MerchantService.IsMerchantValid(merchantDto, merchantAccessCode))
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
            }


            if (!GenericUtility.ValidateSecureIncomingRequest(acceptOrderDetailsRequest.Request, acceptOrderDetailsRequest.CustomHeader.XVerify, merchantDto.SecureSeret))
            {
                throw new OrderException(ResponseCodeConstants.INAVLID_SECURE_SHA);
            }

            OrderDetailsDto parentOrderDetailsDto = await _orderService.GetOrderDetails(parentAggOrderId);
            if (parentOrderDetailsDto == null || parentOrderDetailsDto.MerchantDto == null ||
                parentOrderDetailsDto.OrderTxnInfoDto == null)
            {
                throw new OrderException(ResponseCodeConstants.ORDER_DATA_IS_NOT_VALID);
            }

            if (parentOrderDetailsDto.MerchantDto.MerchantId != merchantId)
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
            }

            if (parentOrderDetailsDto.OrderTxnInfoDto.TxnTypeCode == EnumTxnType.INQUIRY)
            {
                throw new OrderException(-1);
            }
            //validation on the basis of parent txn type
            OrderDetailsDto orderDetailsDto = await _orderService.DoOrderInquiry(parentOrderDetailsDto);
            orderDetailsResponseSentToMerchant = GenericUtility.GetOrderDetailsResponseSentToMerchant(orderDetailsDto);

            orderDetailsResponseSentToMerchant.Code = ResponseCodeConstants.SUCCESS;
            orderDetailsResponseSentToMerchant.Message = _coreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS);


            var base64Converted = GenericUtility.GetBase64FromObject(orderDetailsResponseSentToMerchant);
            var shaGenerated = GenericUtility.GetSHAGenerated(base64Converted, merchantDto.SecureSeret);
            orderDetailsResponseSentToMerchant.CustomHeader = shaGenerated;
            return orderDetailsResponseSentToMerchant;


            //if (parentOrderDetailsDto.OrderTxnInfoDto.TxnTypeCode == EnumTxnType.PURCHASE)
            //{
            //    if (parentOrderDetailsDto.OrderTxnInfoDto.OrderStatusCode != EnumOrderStatus.AUTHORIZING)
            //    {
            //        //call to inquiry service
            //        _orderService.DoOrderInquiry(parentOrderDetailsDto);
            //    }

            //}
            //else if (parentOrderDetailsDto.OrderTxnInfoDto.TxnTypeCode == EnumTxnType.REFUND)
            //{
            //    if (parentOrderDetailsDto.OrderTxnInfoDto.OrderStatusCode == EnumOrderStatus.AUTHORIZING)
            //    {
            //        //call to in quiry service
            //        _orderService.DoOrderInquiry(parentOrderDetailsDto);
            //    }
            //}


        }

        public async Task<OrderDetailsResponseSentToMerchant> DoRefund(IHeaderDictionary headers, AcceptOrderDetailsRequest acceptOrderDetailsRequest)
        {


            if (acceptOrderDetailsRequest == null || string.IsNullOrEmpty(acceptOrderDetailsRequest.Request))
            {
                throw new OrderException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }

            string jsonCustomHeaderString = GenericUtility.GetJsonStringFromHttpRequestHeader(headers);
            if (String.IsNullOrEmpty(jsonCustomHeaderString))
            {
                throw new OrderException(ResponseCodeConstants.ORDER_DATA_IS_NOT_VALID);
            }
            acceptOrderDetailsRequest.CustomHeader = GenericUtility.GetObjectFromJsonString<CustomHeader>(jsonCustomHeaderString);
            string jsonRequestString = GenericUtility.GetJsonStringFromBase64EncodeStriing(acceptOrderDetailsRequest.Request);
            DependentOrderRequest refundRequest = GenericUtility.GetObjectFromJsonString<DependentOrderRequest>(jsonRequestString);

            if (refundRequest == null || GenericUtility.ValidateForXSSAttackAttempt(refundRequest))
            {
                throw new OrderException(ResponseCodeConstants.ORDER_DATA_IS_NOT_VALID);
            }



            OrderDetailsResponseSentToMerchant orderDetailsResponseSentToMerchant = null;

            OrderDetailsDto refundRequestDto = _mapper.Map<OrderDetailsDto>(refundRequest);
            if (refundRequestDto.MerchantDto == null || refundRequestDto.OrderTxnInfoDto == null ||
              refundRequestDto.MerchantDto.MerchantId <= default(int) || String.IsNullOrEmpty(refundRequestDto.MerchantDto.MerchantAccessCode)
             || String.IsNullOrEmpty(refundRequestDto.MerchantDto.MerchantOrderID) || refundRequestDto.OrderTxnInfoDto.AggOrderId <= default(long) || refundRequestDto.OrderTxnInfoDto.Amount <= default(long))
            {
                throw new OrderException(ResponseCodeConstants.ORDER_DATA_IS_NOT_VALID);
            }
            long parentAggOrderId = refundRequestDto.OrderTxnInfoDto.AggOrderId;
            int merchantId = refundRequestDto.MerchantDto.MerchantId;
            string merchantAccessCode = refundRequestDto.MerchantDto.MerchantAccessCode;
            long refundAmount = refundRequestDto.OrderTxnInfoDto.Amount;
            string merchantOrderID = refundRequestDto.MerchantDto.MerchantOrderID;

            MerchantDto merchantDto = await _MerchantService.GetMerchantData(merchantId);
            if (merchantDto == null)
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
            }

            if (!_MerchantService.IsMerchantValid(merchantDto, merchantAccessCode))
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
            }


            if (!GenericUtility.ValidateSecureIncomingRequest(acceptOrderDetailsRequest.Request, acceptOrderDetailsRequest.CustomHeader.XVerify, merchantDto.SecureSeret))
            {
                throw new OrderException(ResponseCodeConstants.INAVLID_SECURE_SHA);
            }

            OrderDetailsDto parentOrderDetailsDto = await _orderService.GetOrderDetails(parentAggOrderId);
            if (parentOrderDetailsDto == null || parentOrderDetailsDto.MerchantDto == null ||
                parentOrderDetailsDto.OrderTxnInfoDto == null)
            {
                throw new OrderException(ResponseCodeConstants.ORDER_DATA_IS_NOT_VALID);
            }

            if (parentOrderDetailsDto.MerchantDto.MerchantId != merchantId)
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
            }
            if (await _MerchantService.IsDuplicateMerchantIDAndMerchantOrderId(merchantId, merchantOrderID))
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_ORDER_ID_ALREADY_EXISTS);
            }

            if (parentOrderDetailsDto.OrderTxnInfoDto.TxnTypeCode != EnumTxnType.PURCHASE)
            {
                throw new OrderException(-1);
            }

            if (!(parentOrderDetailsDto.OrderTxnInfoDto.OrderStatusCode == EnumOrderStatus.CHARGED || parentOrderDetailsDto.OrderTxnInfoDto.OrderStatusCode == EnumOrderStatus.REFUNDED))
            {
                throw new OrderException(-1);
            }

            if ((parentOrderDetailsDto.OrderTxnInfoDto.Amount < refundAmount) || (parentOrderDetailsDto.OrderTxnInfoDto.Amount - parentOrderDetailsDto.OrderTxnInfoDto.RefundAmount < refundAmount))
            {
                throw new OrderException(-1);
            }


            OrderDetailsDto orderDetailsDto = await _orderService.DoOrderRefund(parentOrderDetailsDto, refundRequestDto);


            orderDetailsResponseSentToMerchant = GenericUtility.GetOrderDetailsResponseSentToMerchant(orderDetailsDto);

            orderDetailsResponseSentToMerchant.Code = ResponseCodeConstants.SUCCESS;
            orderDetailsResponseSentToMerchant.Message = _coreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS);

            var base64Converted = GenericUtility.GetBase64FromObject(orderDetailsResponseSentToMerchant);
            var shaGenerated = GenericUtility.GetSHAGenerated(base64Converted, merchantDto.SecureSeret);
            orderDetailsResponseSentToMerchant.CustomHeader = shaGenerated;
            // return shaGenerated;

            return orderDetailsResponseSentToMerchant;




            /*
             * Inquiry
             *      Purchase:(parent order id)
                 *      merchant data  (merchant id ,merchant order id)
                 *      Order txn data(transaction type, agg order id,payment id,order status,order response code ,amount,refund amount)
                 *      payment data (payment mode , gateway name,gateway payment id,gateway payment id,gatway status)
                 *           acquirer :bank txn,auth rrn
                 `
             *   Inquiry:
             *       Refund:(parent order id)
             *        merchant data  (merchant id ,merchant order id)
                 *      Order txn data(transaction type, agg order id,payment id,order status,order response code ,amount,refund amount,parent agg order id,parent order response code,parent response status)
                 *      payment data (gateway name,gateway payment id,gateway payment id,gatway status,)
                 *           acquirer :bank txn,auth rrn
             *            
             *   
             *      
             * 
             * 
             * 
             */






        }

        private async Task<DoPaymentRequest> CheckSavedCardOrNotAsync(OrderDetailsDto orderDetailsDto, DoPaymentRequest doPaymentRequest)
        {
            CardRequest cardRequest;
            SavedCardRequest savedCardRequest = new SavedCardRequest();
            savedCardRequest.cardRequest = _mapper.Map<CardRequest>(doPaymentRequest.CardRequest);
            savedCardRequest.merchantRequest = _mapper.Map<MerchantRequest>(doPaymentRequest.MerchantDetailsRequest);
            savedCardRequest.customerRequest = _mapper.Map<CustomerRequest>(orderDetailsDto.CustomerDto);
            if (doPaymentRequest.CardRequest.IsCardToBeSave == true)
            {
                await _customerSavedCardValidation.InsertCustomerSavedCardValidation(savedCardRequest);
            }
            if (doPaymentRequest.CardRequest.CardNumber == null)
            {
                CardRequest[] cardRequests = await _customerSavedCardValidation.GetAllSaveCard(savedCardRequest);
                for (int specificCard = 0; specificCard < cardRequests.Length; specificCard++)
                {
                    if (cardRequests[specificCard].SavedCardId == doPaymentRequest.CardRequest.SavedCardId)
                    {
                        cardRequests[specificCard].CVV = doPaymentRequest.CardRequest.CVV;
                        doPaymentRequest.CardRequest = _mapper.Map<CardRequest>(cardRequests[specificCard]);
                      
                        break;
                    }
                }

            }
            return doPaymentRequest;
        }
    }
}