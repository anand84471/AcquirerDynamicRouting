using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using Core.Utilities;
using ExpressCheckout.BusinessLayer.Abstract;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using ExpressCheckoutPaymentIntegrationModule.Factory.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;
using Microsoft.AspNetCore.Http;

namespace ExpressCheckout.BusinessLayer.Concrete
{
    public class PayUValidation : IPayUValidation
    {

        private readonly IOrderService orderService;
        private readonly IPaymentIntegrationModuleFactory _paymentIntegrationModuleFactory;
        private readonly IGatewayService _gateWayService;
        private readonly IMerchantService merchantService;
        private readonly ICommonIntegrationHandlerService commonIntegrationHandlerService;

        public PayUValidation(IOrderService orderService, IGatewayService _gateWayService,
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
        public async Task<string> CompletePayUTransaction(IFormCollection formCollection)
        {
            string command = "check_payment";
            string paymentId = formCollection["mihpayid"].ToString();
            string gatewayorderid = formCollection["txnid"];          
           
            string[] merc_hash_vars_seq;
            string merc_hash_string = string.Empty;
            string hash_seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

            var orderPaymentDetails = await this.GetOrderPaymentDetails(EnumGateway.PayU, gatewayorderid);

            var orderDetails = await this.orderService.GetOrderDetails(orderPaymentDetails.AggOrderId);

            MerchantDto merchantDto = await merchantService.GetMerchantData(orderDetails.MerchantDto.MerchantId);
            if (merchantDto == null)
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
            }
            var merchantGatewayConfig = await merchantService.GetMerchantPaymentGatewayConfigurationDetails(merchantDto.MerchantId, (int)EnumGateway.PayU);
            if (merchantGatewayConfig == null)
            {
                throw new OrderException(ResponseCodeConstants.MERCHANT_CONFIG_NOT_PRESENT);
            }
            orderDetails.MerchantGatewayConfigurationMappingDto = merchantGatewayConfig;

            string hashenquiry = orderDetails.MerchantGatewayConfigurationMappingDto.SecretKeyIssuedByGatewayToMerchant + "|" +
                command + "|" + paymentId + "|" + orderDetails.MerchantGatewayConfigurationMappingDto.PasswordIssuedByGatewayToMerchant;

                merc_hash_vars_seq = hash_seq.Split('|');
                Array.Reverse(merc_hash_vars_seq);
                merc_hash_string = formCollection["additionalCharges"].ToString() + "|" + orderDetails.MerchantGatewayConfigurationMappingDto.PasswordIssuedByGatewayToMerchant + "|" + formCollection["status"].ToString();


                foreach (string merc_hash_var in merc_hash_vars_seq)
                {
                    merc_hash_string += "|";
                    merc_hash_string = merc_hash_string + (formCollection[merc_hash_var].ToString() != null ?
                        formCollection[merc_hash_var].ToString() : "");

                }


                string hashenquiry1 = GenericUtility.GetHash512(hashenquiry).ToLower();
                string merc_hash = GenericUtility.GetHash512(merc_hash_string).ToLower();
                if (merc_hash != formCollection["hash"])
                {
                //Hash value did not matched
                throw new OrderException(ResponseCodeConstants.INAVLID_SECURE_SHA);

                }
                else
                {
                    gatewayorderid = formCollection["txnid"];
                   
                }

                var inquiryResposneFromPayu = await DoInquiryAtPayU(paymentId, orderDetails);

                var finalResponseHelperDto = new FinalResponseHelperDto
                {
                    AggOrderId = orderPaymentDetails.AggOrderId,
                    AggPaymentId = orderPaymentDetails.AggPaymentId,
                    GatewayPaymentId = paymentId,
                    FinalResponseFromGateway = inquiryResposneFromPayu.ResponseCodeRecievedFromGateway,
                    OrderStatus = inquiryResposneFromPayu.ResponseCodeRecievedFromGateway.Equals("captured") ? EnumOrderStatus.CHARGED : EnumOrderStatus.AUTHORIZING,
                    MerchantId = merchantDto.MerchantId,
                    OrderResponseCode = inquiryResposneFromPayu.ResponseCodeRecievedFromGateway.Equals("captured") ? ResponseCodeConstants.SUCCESS : ResponseCodeConstants.FAILURE
                };      

                return await this.commonIntegrationHandlerService.UpdateTransactionResponseAndCreateResponseForMerchant(finalResponseHelperDto);
            
          
        }


        private async Task<InquiryResponseDto> DoInquiryAtPayU(string paymentIDFromGateway, OrderDetailsDto orderDetails)
        {
            var orderDetailsDto = new OrderDetailsDto
            {
                PaymentDataDto = new PaymentDataDto
                {
                    PaymentIdGeneratedByGateway = paymentIDFromGateway
                }
            };

            var taskGatewayDto = _gateWayService.GetGatewayDetails((int)EnumGateway.PayU);
            var taskMerchantGatewayConfigurationMappingDto = merchantService.GetMerchantPaymentGatewayConfigurationDetails(orderDetails.MerchantDto.MerchantId, (int)EnumGateway.PayU);
            var paymentGatewayHandlerService = _paymentIntegrationModuleFactory.GetPaymentGatewayHandlerService(EnumGateway.PayU);
            return await paymentGatewayHandlerService.DoInquiryOfPurchase(orderDetailsDto, await taskGatewayDto, await taskMerchantGatewayConfigurationMappingDto);
        }
    }
}
