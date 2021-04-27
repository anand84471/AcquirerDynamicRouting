using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Core.Cache.Abstract;
using Core.Configuration;
using Core.Constants;
using Core.Features.ExceptionHandling.Abstract;
using Core.Utilities;
using ExpressCheckout.BusinessLayer.Abstract;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Abstract;
using ExpressCheckoutContracts.Response.Concrete;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ExpressCheckout.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ICoreCache _coreCache;
        private readonly IOrderValidation _orderValidation;
        private readonly ICommonIntegrationHandlerService commonIntegrationHandlerService;

        /// <summary>
        /// Customer Api Constructor
        /// </summary>
        /// <param name="expressCheckoutCustomerValidation">Gate business layer for validations</param>
        public OrderController(IOrderValidation orderValidation, ICoreCache coreCache, ICommonIntegrationHandlerService commonIntegrationHandlerService)
        {
            _coreCache = coreCache;
            _orderValidation = orderValidation;
            this.commonIntegrationHandlerService = commonIntegrationHandlerService;
        }

        [Route("create")]
        public async Task<MasterResponse> CreatePurchaseOrdrer(AcceptOrderDetailsRequest acceptOrderDetailsRequest)
        {
            OrderCreateResponse response = null;
            long AggOrderId = await _orderValidation.CreatePurchaseOrder(Request.Headers, acceptOrderDetailsRequest);
            return response = new OrderCreateResponse
            {
                Code = ResponseCodeConstants.SUCCESS,
                Message = _coreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS),
                AggOrderId = AggOrderId
            };
        }
        [Route("update")]
        public async Task<MasterResponse> UpdatePurchaseOrdrer(AcceptOrderDetailsRequest acceptOrderDetailsRequest)
        {
            OrderCreateResponse response = null;
            bool AggOrderId = await _orderValidation.UpdatePurchaseOrdrer(Request.Headers,acceptOrderDetailsRequest);
            return response = new OrderCreateResponse
            {
                Code = ResponseCodeConstants.SUCCESS,
                Message = _coreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS),

            };
        }

        [Route("status")]
        public async Task<MasterResponse> DoInquiry(AcceptOrderDetailsRequest acceptOrderDetailsRequest)
        {
            //var data = await _orderValidation.DoInquiry(inquiryRequest);
            //return new InquiryResponse
            //{
            //    Code = ResponseCodeConstants.SUCCESS,
            //    Message = _coreCache.GetResponseMsg(ResponseCodeConstants.SUCCESS),
            //    OrderDetails = data
            //};

            OrderDetailsResponseSentToMerchant orderDetailsResponseSentToMerchant = await _orderValidation.DoInquiry(Request.Headers,acceptOrderDetailsRequest);
            Response.Headers.Add("X-VERIFY", orderDetailsResponseSentToMerchant.CustomHeader);
            return orderDetailsResponseSentToMerchant;

        }

        [Route("refund")]
        public async Task<MasterResponse> DoRefund(AcceptOrderDetailsRequest acceptOrderDetailsRequest)
        {

            OrderDetailsResponseSentToMerchant orderDetailsResponseSentToMerchant = await _orderValidation.DoRefund(Request.Headers,acceptOrderDetailsRequest);
            Response.Headers.Add("X-VERIFY", orderDetailsResponseSentToMerchant.CustomHeader);
            return orderDetailsResponseSentToMerchant;
        }

        [Route("dopayment/{orderId}")]
        public async Task<ActionResult> DoPayment(long orderId, [FromBody]DoPaymentRequest doPaymentRequest)
        {
            string response = string.Empty;
            try
            {
                response = await _orderValidation.DoPayment(orderId, doPaymentRequest);
            }
            catch (Exception ex)
            {
                 if (ex is MasterException)
                {
                    var actualEx = ex as MasterException;
                    var finalResponseDto = new FinalResponseHelperDto
                    {
                        AggOrderId = orderId,
                        MerchantId = doPaymentRequest.MerchantDetailsRequest.MerchantId,
                        OrderStatus = ExpressCheckoutContracts.Enums.EnumOrderStatus.FAILED,
                        OrderResponseCode = actualEx.ResponseCode
                    };
                    var sha = await this.commonIntegrationHandlerService.UpdateTransactionResponseAndCreateResponseForMerchant(finalResponseDto);
                    NameValueCollection nv = new NameValueCollection();
                    nv.Add("sha", sha);
                    response = GenericCoreUtility.CreateFormToPost(doPaymentRequest.MerchantDetailsRequest.MerchantReturnUrl, nv);
                }
                else
                {
                    // error view
                }
            }
            return Content(response);
        }



    }
}