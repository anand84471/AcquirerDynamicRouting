using System;
using System.Collections.Specialized;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using AutoMapper;
using Core.Constants;
using Core.Utilities;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.PayU;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ApiClients.Abstract;
using ExpressCheckoutPaymentIntegrationModule.Factory.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;
using PinePGController.ExceptionHandling.CustomExceptions;

namespace ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Concrete.PayU
{
    public class PayuIntegrationHandlerService : IGatewayIntegrationHandlerService
    {
        private readonly IPaymentIntegrationModuleFactory paymentIntegrationModuleFactory;      
        private readonly IOrderRepo _orderRepo;

        public PayuIntegrationHandlerService(IPaymentIntegrationModuleFactory paymentIntegrationModuleFactory, IOrderRepo _orderRepo)
        {
            this.paymentIntegrationModuleFactory = paymentIntegrationModuleFactory;
            this._orderRepo = _orderRepo;
        }



        private NameValueCollection CreateInquiryRequest(OrderDetailsDto orderDetailsDto)
        {
            string txnid = orderDetailsDto.PaymentDataDto.OrderIdGeneratedByGateway;
            NameValueCollection nv = new NameValueCollection();
            nv.Add("key", orderDetailsDto.MerchantGatewayConfigurationMappingDto.SecretKeyIssuedByGatewayToMerchant);
            nv.Add("command", "check_payment"); //Convert.ToString(orderDetailsDto.PaymentDataDto.PaymentId)+);
            nv.Add("var1", orderDetailsDto.PaymentDataDto.PaymentIdGeneratedByGateway);
            nv.Add("hash", GetHashForPayU(orderDetailsDto, "check_payment"));
            return nv;
        }


        private NameValueCollection CreateRefundInquiryRequest(OrderDetailsDto orderDetailsDto)
        {
            string txnid = orderDetailsDto.PaymentDataDto.OrderIdGeneratedByGateway;
            NameValueCollection nv = new NameValueCollection();
            nv.Add("key", orderDetailsDto.MerchantGatewayConfigurationMappingDto.SecretKeyIssuedByGatewayToMerchant);
            nv.Add("command", "check_action_status"); 
            nv.Add("var1", orderDetailsDto.PaymentDataDto.PaymentIdGeneratedByGateway);
            nv.Add("hash", GetHashForPayU(orderDetailsDto, "check_action_status"));
            return nv;
        }


        private NameValueCollection CreateRefundRequest(OrderDetailsDto parentorderDetailsDto, long AmountToBeRefunded)
        {
            string txnid = this.Generatetxnid();
            NameValueCollection nv = new NameValueCollection();
            nv.Add("key", parentorderDetailsDto.MerchantGatewayConfigurationMappingDto.SecretKeyIssuedByGatewayToMerchant);
            nv.Add("command", "cancel_refund_transaction"); //Convert.ToString(orderDetailsDto.PaymentDataDto.PaymentId)+);
            nv.Add("var1", parentorderDetailsDto.PaymentDataDto.PaymentIdGeneratedByGateway);
            nv.Add("var2", txnid);
            nv.Add("var3", Convert.ToString(AmountToBeRefunded));
            nv.Add("hash", GetHashForPayU(parentorderDetailsDto, "cancel_refund_transaction"));
            return nv;
        }

        private string GetHashForPayU(OrderDetailsDto orderDetails, string command)
        {

            string hashString = orderDetails.MerchantGatewayConfigurationMappingDto.SecretKeyIssuedByGatewayToMerchant + "|"
                              + command + "|" +
                              orderDetails.PaymentDataDto.PaymentIdGeneratedByGateway + "|" +
                              orderDetails.MerchantGatewayConfigurationMappingDto.PasswordIssuedByGatewayToMerchant;

            return GenericUtility.GetHash512(hashString);
        }

        //private string GetHashForInquiryPayU(OrderDetailsDto orderDetails)
        //{

        //    string hashString = orderDetails.MerchantGatewayConfigurationMappingDto.SecretKeyIssuedByGatewayToMerchant + "|"
        //                      + "check_payment" + "|" +
        //                      orderDetails.PaymentDataDto.PaymentIdGeneratedByGateway + "|" +
        //                      orderDetails.MerchantGatewayConfigurationMappingDto.PasswordIssuedByGatewayToMerchant;

        //    return GenericUtility.GetHash512(hashString);
        //}


        //private string GetHashForRefundInquiryPayU(OrderDetailsDto orderDetails)
        //{

        //    string hashString = orderDetails.MerchantGatewayConfigurationMappingDto.SecretKeyIssuedByGatewayToMerchant + "|"
        //                      + "check_action_status" + "|" +
        //                      orderDetails.PaymentDataDto.PaymentIdGeneratedByGateway + "|" +
        //                      orderDetails.MerchantGatewayConfigurationMappingDto.PasswordIssuedByGatewayToMerchant;

        //    return GenericUtility.GetHash512(hashString);
        //}

        //private string GetHashForRefundPayU(OrderDetailsDto orderDetails, string txnId, long AmountToBeRefunded)
        //{

        //    //string hashString = orderDetails.MerchantGatewayConfigurationMappingDto.SecretKeyIssuedByGatewayToMerchant + "|"
        //    //                  + "cancel_refund_transaction" + "|" +
        //    //                  orderDetails.PaymentDataDto.PaymentIdGeneratedByGateway + "|" +
        //    //                  txnId+"|"+
        //    //                  Convert.ToString(AmountToBeRefunded)+"|"+
        //    // orderDetails.MerchantGatewayConfigurationMappingDto.PasswordIssuedByGatewayToMerchant;
        //    //  return GenericUtility.GetHash512(hashString);


        //    string hashString = orderDetails.MerchantGatewayConfigurationMappingDto.SecretKeyIssuedByGatewayToMerchant + "|"
        //                      + "cancel_refund_transaction" + "|" +
        //                      orderDetails.PaymentDataDto.PaymentIdGeneratedByGateway + "|" +
        //                      orderDetails.MerchantGatewayConfigurationMappingDto.PasswordIssuedByGatewayToMerchant;

        //    return GenericUtility.GetHash512(hashString);



        //}

        public async Task<InquiryResponseDto> DoInquiryOfPurchase(OrderDetailsDto orderDetailsDto, GatewayDto gatewayDto, MerchantGatewayConfigurationMappingDto merchantGatewayConfigurationMappingDto)
        {
            orderDetailsDto.GatewayDto = gatewayDto;
            orderDetailsDto.MerchantGatewayConfigurationMappingDto = merchantGatewayConfigurationMappingDto;
            var payuCCDCRequest = this.CreateInquiryRequest(orderDetailsDto);
            GenericCoreUtility.CreateFormToPost(orderDetailsDto.GatewayDto.InquiryUrl, payuCCDCRequest);
            string result;
            using (WebClient client = new WebClient())
            {
                byte[] response = client.UploadValues(orderDetailsDto.GatewayDto.InquiryUrl, "POST", payuCCDCRequest);
                result = System.Text.Encoding.UTF8.GetString(response);

            }

            InquiryResponseDto inquiryResponseDto = new InquiryResponseDto();
            inquiryResponseDto.IsRequestSend = true;

            PayUInquiryResponseDto payUInquiryResponseDto = GenericUtility.ConvertJsonStringToObject<PayUInquiryResponseDto>(result);
            inquiryResponseDto.IsResponseReceived = true;
            if (payUInquiryResponseDto != null && payUInquiryResponseDto.transactionDetails != null)
            {
                inquiryResponseDto.ResponseRecievedFromGateway = GenericUtility.ConvertObjectToJsonString<PayUInquiryResponseDto>(payUInquiryResponseDto);
                inquiryResponseDto.ResponseCodeRecievedFromGateway = payUInquiryResponseDto.transactionDetails.UnMappedStatus;
                inquiryResponseDto.DependentPaymentTxnOrderResponseCode = ResponseCodeConstants.SUCCESS;
                inquiryResponseDto.DependentPaymentTxnOrderStatusCode = EnumOrderStatus.INQUIRY_COMPLETED;

                if (!string.IsNullOrEmpty(payUInquiryResponseDto.Status) &&
                    payUInquiryResponseDto.Status.Equals(ResponseCodeConstants.SUCCESS)
                    &&
                    payUInquiryResponseDto.Status.Equals("captured", System.StringComparison.OrdinalIgnoreCase))
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

        public async Task<InquiryResponseDto> DoInquiryOfRefund(OrderDetailsDto parentOrderDetailsDto, GatewayDto gatewayDto, MerchantGatewayConfigurationMappingDto merchantGatewayConfigurationMappingDto)
        {
            parentOrderDetailsDto.GatewayDto = gatewayDto;
            parentOrderDetailsDto.MerchantGatewayConfigurationMappingDto = merchantGatewayConfigurationMappingDto;
            parentOrderDetailsDto.PaymentDataDto.PaymentIdGeneratedByGateway = "122333";
            var payuInqyuiryRequest = this.CreateRefundInquiryRequest(parentOrderDetailsDto);

            string result;
            using (WebClient client = new WebClient())
            {
                byte[] response = client.UploadValues(parentOrderDetailsDto.GatewayDto.InquiryUrl, "POST", payuInqyuiryRequest);
                result = System.Text.Encoding.UTF8.GetString(response);

            }

            InquiryResponseDto inquiryResponseDto = new InquiryResponseDto();
            inquiryResponseDto.IsRequestSend = true;

            PayURefundInquiryResponseDTO payUInquiryResponseDto = GenericUtility.ConvertJsonStringToObject<PayURefundInquiryResponseDTO>(result);
            inquiryResponseDto.IsResponseReceived = true;
            if (payUInquiryResponseDto != null && payUInquiryResponseDto.transactionDetails != null)
            {
                //inquiryResponseDto.ResponseRecievedFromGateway = GenericUtility.ConvertObjectToJsonString<PayUInquiryResponseDto>(payUInquiryResponseDto);
                inquiryResponseDto.ResponseCodeRecievedFromGateway = payUInquiryResponseDto.Status;
                inquiryResponseDto.DependentPaymentTxnOrderResponseCode = ResponseCodeConstants.SUCCESS;
                inquiryResponseDto.DependentPaymentTxnOrderStatusCode = EnumOrderStatus.INQUIRY_COMPLETED;

                if (!string.IsNullOrEmpty(payUInquiryResponseDto.Status) &&
                    payUInquiryResponseDto.Status.Equals(ResponseCodeConstants.SUCCESS)
                    && payUInquiryResponseDto.transactionDetails != null && payUInquiryResponseDto.transactionDetails.ContainsKey(parentOrderDetailsDto.PaymentDataDto.PaymentIdGeneratedByGateway)
                  )
                {

                    InquiryTransactionDetails inquiryTransactionDetails =
                        (InquiryTransactionDetails)payUInquiryResponseDto.transactionDetails[parentOrderDetailsDto.PaymentDataDto.PaymentIdGeneratedByGateway];
                    if (inquiryTransactionDetails.Status.Equals("success", StringComparison.OrdinalIgnoreCase))
                    {
                        inquiryResponseDto.IsParentOrderIdTobeUpdate = true;
                        inquiryResponseDto.ParentOrderIdOrderStatusTobeUpdate = (int)EnumOrderStatus.REFUNDED;
                        inquiryResponseDto.ParentOrderIdReponseCodeTobeUpdate = ResponseCodeConstants.SUCCESS;
                        inquiryResponseDto.ParentOrderIdResponseRecievedFromGatewayToBeUpdate = inquiryResponseDto.ResponseRecievedFromGateway;

                    }

                }

            }
            else
            {
                inquiryResponseDto.DependentPaymentTxnOrderResponseCode = ResponseCodeConstants.FAILURE;
                inquiryResponseDto.DependentPaymentTxnOrderStatusCode = EnumOrderStatus.GATEWAY_ERROR;


            }
            return inquiryResponseDto;
        }


        public string Generatetxnid()
        {
            string strHash = DateTime.Now.ToString("ddMMyyyyHHmmssfffff");           
            return strHash;
        }
        public async Task<string> DoPurchase(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetailsDto)
        {
            string strGatewayOrderID = Generatetxnid() + orderDetailsDto.PaymentDataDto.PaymentId;

            var orderStepDto = new OrderStepDto
            {
                PaymentId = orderDetailsDto.PaymentDataDto.PaymentId,
                RequestType = 2
            };

            await this._orderRepo.InsertOrderPaymentStepDetails(orderStepDto);
            var updatedOrderPaymentDetail = new OrderPaymentDetailsDto
            {
                AggPaymentId = orderDetailsDto.PaymentDataDto.PaymentId,
                GatewayOrderID = strGatewayOrderID
            };
            if (!await this._orderRepo.UpdateOrderPaymentDetails(updatedOrderPaymentDetail))
            {
                throw new DBException(ResponseCodeConstants.FAILURE);
            }

            var orderToBeUpdated = new UpdateOrderDetailsDto
            {
                AggOrderId = orderDetailsDto.OrderTxnInfoDto.AggOrderId,
                PaymentId = orderDetailsDto.PaymentDataDto.PaymentId,
                GatewayId = (int)EnumGateway.PayU
            };
            if (!await this._orderRepo.UpdateOrderDetails(orderToBeUpdated))
            {
                throw new DBException(ResponseCodeConstants.FAILURE);
            }
            orderDetailsDto.PaymentDataDto.OrderIdGeneratedByGateway = strGatewayOrderID;

            var payuPaymentModeHandlerService = this.paymentIntegrationModuleFactory.GetPayuPaymentModeHandlers(doPaymentRequest.PaymentDetailsRequest.PaymentMode);
            return await payuPaymentModeHandlerService.DoPayment(doPaymentRequest, orderDetailsDto);
        }
        public async Task<RefundResponseDto> DoRefund(OrderDetailsDto parentOrderDetailsDto, long AmountToBeRefunded, GatewayDto gatewayDto, MerchantGatewayConfigurationMappingDto merchantGatewayConfigurationMappingDto)
        {

            parentOrderDetailsDto.MerchantGatewayConfigurationMappingDto = merchantGatewayConfigurationMappingDto;
            parentOrderDetailsDto.GatewayDto = gatewayDto;

            var payuRefundRequest = this.CreateRefundRequest(parentOrderDetailsDto, AmountToBeRefunded);

            string result;
            using (WebClient client = new WebClient())
            {
                byte[] response = client.UploadValues(parentOrderDetailsDto.GatewayDto.RefundUrl, "POST", payuRefundRequest);
                result = System.Text.Encoding.UTF8.GetString(response);

            }


            RefundResponseDto refundResponseDto = new RefundResponseDto();
            refundResponseDto.IsRequestSend = true;


            PayURefundResponseDTO payURefundResponseDto =
                GenericUtility.ConvertJsonStringToObject<PayURefundResponseDTO>(result);
            refundResponseDto.IsResponseReceived = true;


            if (payURefundResponseDto != null)
            {
                refundResponseDto.ResponseRecievedFromGateway = result;
                refundResponseDto.ResponseCodeRecievedFromateway = payURefundResponseDto.Status;

                /*
                 * 
                    pending: This state indicates that Razorpay is attempting to process the refund.
                    processed: This is the terminal state of the refund.
  
                 * 
                 * 
                 */
                if (!string.IsNullOrEmpty(payURefundResponseDto.Status) &&
                    payURefundResponseDto.Status.Equals(ResponseCodeConstants.SUCCESS.ToString())
                    )
                {
                    refundResponseDto.IsParentOrderIdTobeUpdate = true;
                    refundResponseDto.ParentOrderIdOrderStatusTobeUpdate = (int)EnumOrderStatus.REFUNDED;
                    refundResponseDto.ParentOrderIdReponseCodeTobeUpdate = ResponseCodeConstants.SUCCESS;

                    refundResponseDto.GatewayPaymentId = payURefundResponseDto.BankRefNum;
                    refundResponseDto.GatewayOrderID = payuRefundRequest["var2"];
                    refundResponseDto.DependentPaymentTxnOrderResponseCode = ResponseCodeConstants.SUCCESS;
                    refundResponseDto.DependentPaymentTxnOrderStatusCode = EnumOrderStatus.REFUNDED;
                }
                else
                {
                    refundResponseDto.ResponseCodeRecievedFromateway = payURefundResponseDto.ErrorCode;
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
    }
}
