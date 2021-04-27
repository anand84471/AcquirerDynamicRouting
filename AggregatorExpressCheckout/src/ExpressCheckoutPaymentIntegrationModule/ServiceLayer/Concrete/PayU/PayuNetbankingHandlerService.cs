using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;

namespace ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Concrete.PayU
{
    public class PayuNetbankingHandlerService : IPayuPaymentModeHandlers
    {
        private readonly INetbankingRepo netbankingRepo;
        private readonly IOrderRepo orderRepo;

        public PayuNetbankingHandlerService(INetbankingRepo netbankingRepo, IOrderRepo orderRepo)
        {
            this.netbankingRepo = netbankingRepo;
            this.orderRepo = orderRepo;
        }
        public async Task<string> DoPayment(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetailsDto)
        {
            var payuNetBankingRequest = await this.CreateNetbankingRequest(doPaymentRequest, orderDetailsDto);
            return GenericCoreUtility.CreateFormToPost(orderDetailsDto.GatewayDto.CardPaymentUrl, payuNetBankingRequest);
        }




        private async Task<NameValueCollection> CreateNetbankingRequest(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetailsDto)
        {
            var paymentOptionCode = await this.netbankingRepo.GetNetbankingPaymentOptionCode(EnumGateway.PayU, doPaymentRequest.NetbankingRequest.PaymentCode);
            string txnid = orderDetailsDto.PaymentDataDto.OrderIdGeneratedByGateway;
            NameValueCollection nv = new NameValueCollection();
            nv.Add("key", orderDetailsDto.MerchantGatewayConfigurationMappingDto.SecretKeyIssuedByGatewayToMerchant);
            nv.Add("txnid", txnid); //Convert.ToString(orderDetailsDto.PaymentDataDto.PaymentId)+);
            nv.Add("amount", Convert.ToString(orderDetailsDto.OrderTxnInfoDto.Amount));
            nv.Add("productinfo", "productinfo");
            nv.Add("firstname", orderDetailsDto.CustomerDto == null ? "" : orderDetailsDto.CustomerDto.FirstName);
            nv.Add("phone", orderDetailsDto.CustomerDto == null ? "" : orderDetailsDto.CustomerDto.MobileNumber);
            nv.Add("pg", "NB");
            nv.Add("bankcode", paymentOptionCode);
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
                              + //orderDetails.PaymentDataDto.PaymentId +
                              txnId +
                              "|" + orderDetails.OrderTxnInfoDto.Amount + "|" + "productinfo" +
                              "|"
                              //+ orderDetails.CustomerDto == null ? "" : 
                              + orderDetails.CustomerDto.FirstName + "|"
                             // + orderDetails.CustomerDto == null ? "" :
                             + orderDetails.CustomerDto.EmailId + "|||||||||||"
                              + orderDetails.MerchantGatewayConfigurationMappingDto.PasswordIssuedByGatewayToMerchant;


            return GenericUtility.GetHash512(hashString);
        }


        //public async Task<string> DoInquiry(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetailsDto)
        //{
        //    //var payuCCDCRequest = this.CreateNetbankingRequest(doPaymentRequest, orderDetailsDto);
        //    //return GenericCoreUtility.CreateFormToPost(orderDetailsDto.GatewayDto.CardPaymentUrl, payuCCDCRequest);
        //}
    }
}
