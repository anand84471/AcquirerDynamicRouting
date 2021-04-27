using Core.Utilities;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Concrete.PayU
{
    public class PayuCCDCHandlerService : IPayuPaymentModeHandlers
    {
        public async Task<string> DoPayment(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetailsDto)
        {
            var payuCCDCRequest = this.CreateCCDCRequest(doPaymentRequest, orderDetailsDto);
            return GenericCoreUtility.CreateFormToPost(orderDetailsDto.GatewayDto.CardPaymentUrl, payuCCDCRequest);
        }


        private NameValueCollection CreateCCDCRequest(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetailsDto)
        {
            string txnid = orderDetailsDto.PaymentDataDto.OrderIdGeneratedByGateway;
            NameValueCollection nv = new NameValueCollection();
            nv.Add("key", orderDetailsDto.MerchantGatewayConfigurationMappingDto.SecretKeyIssuedByGatewayToMerchant);
            nv.Add("txnid", txnid); //Convert.ToString(orderDetailsDto.PaymentDataDto.PaymentId)+);
            nv.Add("amount", Convert.ToString(orderDetailsDto.OrderTxnInfoDto.Amount));
            nv.Add("productinfo", "productinfo");
            nv.Add("firstname", orderDetailsDto.CustomerDto == null ? "" : orderDetailsDto.CustomerDto.FirstName);
            nv.Add("phone", orderDetailsDto.CustomerDto == null ? "" : orderDetailsDto.CustomerDto.MobileNumber);
            nv.Add("pg", "CC");
            nv.Add("ccnum", doPaymentRequest.CardRequest.CardNumber);
            nv.Add("ccname", doPaymentRequest.CardRequest.CardHolderName);
            nv.Add("ccexpmon", doPaymentRequest.CardRequest.CardExpiryMonth);
            nv.Add("ccvv", doPaymentRequest.CardRequest.CVV);
            nv.Add("ccexpyr", doPaymentRequest.CardRequest.CardExpiryYear);
            nv.Add("email", orderDetailsDto.CustomerDto == null ? "" : orderDetailsDto.CustomerDto.EmailId);
            nv.Add("curl", orderDetailsDto.GatewayDto.ResponseReturnedUrl);
            nv.Add("surl", orderDetailsDto.GatewayDto.ResponseReturnedUrl);
            nv.Add("furl", orderDetailsDto.GatewayDto.ResponseReturnedUrl);
            nv.Add("hash", GetHashForPayU(orderDetailsDto, txnid));
            return nv;
        }

        private string GetHashForPayU(OrderDetailsDto orderDetails, string txnId)
        {
            string hashString = orderDetails.MerchantGatewayConfigurationMappingDto.SecretKeyIssuedByGatewayToMerchant + "|"
                             + txnId +
                              "|" + orderDetails.OrderTxnInfoDto.Amount + "|" + "productinfo" +
                              "|"
                              + (orderDetails.CustomerDto != null ? orderDetails.CustomerDto.FirstName : "") + "|"
                              + (orderDetails.CustomerDto != null ? orderDetails.CustomerDto.EmailId : "" )+ 
                               "|||||||||||"
                              + orderDetails.MerchantGatewayConfigurationMappingDto.PasswordIssuedByGatewayToMerchant;





            return GenericUtility.GetHash512(hashString);
        }


        public async Task<string> DoInquiry(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetailsDto)
        {
            var payuCCDCRequest = this.CreateCCDCRequest(doPaymentRequest, orderDetailsDto);
            return GenericCoreUtility.CreateFormToPost(orderDetailsDto.GatewayDto.CardPaymentUrl, payuCCDCRequest);
        }
    }
}
