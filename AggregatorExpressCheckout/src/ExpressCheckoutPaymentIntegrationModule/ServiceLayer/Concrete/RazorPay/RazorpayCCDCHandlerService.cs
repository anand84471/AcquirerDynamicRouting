using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using ExpressCheckoutContracts.Constants;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.Razorpay;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ApiClients.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Concrete.RazorPay
{
    public class RazorpayCCDCHandlerService : IRazorpayPaymentModeHandlers
    {
        private readonly IPaymentIntegrationApiClient apiClient;
        private readonly IOrderRepo orderRepo;

        public RazorpayCCDCHandlerService(IPaymentIntegrationApiClient apiClient, IOrderRepo orderRepo)
        {
            this.apiClient = apiClient;
            this.orderRepo = orderRepo;
        }

        public async Task<string> DoPayment(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetailsDto, RazorpayOrderResponseDto razorpayOrderResponseDto)
        {
            var ccdcRequest = CreateCCDCrequest(doPaymentRequest, orderDetailsDto, razorpayOrderResponseDto);
            var response = await this.apiClient.DoPaymentAtRazorpay(ccdcRequest, orderDetailsDto.MerchantGatewayConfigurationMappingDto, orderDetailsDto.GatewayDto.CardPaymentUrl);
            if (response == null)
            {
                throw new OrderException(ResponseCodeConstants.PAYMENT_FAILED_AT_AGGREGATOR);
            }

            ccdcRequest.Card.CVV = "***";

            var orderStepDto = new OrderStepDto
            {
                PaymentId = orderDetailsDto.PaymentDataDto.PaymentId,
                RequestSendToGateway = JsonConvert.SerializeObject(ccdcRequest),
                RequestType = 3,
                ResponseReceivedFromGateway = null,
            };

            await this.orderRepo.InsertOrderPaymentStepDetails(orderStepDto);

            return response;
        }

        private RazorpayCCDCRequestDto CreateCCDCrequest(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetailsDto, RazorpayOrderResponseDto razorpayOrderResponseDto)
        {
            var ccdcRequest = new RazorpayCCDCRequestDto
            {
                KeyId = orderDetailsDto.MerchantGatewayConfigurationMappingDto.MerchantIdIssuedByGatewayToMerchant,
                Amount = orderDetailsDto.OrderTxnInfoDto.Amount,
                OrderId = razorpayOrderResponseDto.Id,
                Currency = razorpayOrderResponseDto.Currency,
                Method = CommonConstants.RAZORPAY_CARD_METHOD,
                Contact = orderDetailsDto.CustomerDto.MobileNumber,
                Email = orderDetailsDto.CustomerDto.EmailId,
                Card = new RazorpayCardDataDto
                {
                    CardNumber = doPaymentRequest.CardRequest.CardNumber,
                    CardHolderName = doPaymentRequest.CardRequest.CardHolderName,
                    CardExpiryMonth = doPaymentRequest.CardRequest.CardExpiryMonth,
                    CardExpiryYear = doPaymentRequest.CardRequest.CardExpiryYear,
                    CVV = doPaymentRequest.CardRequest.CVV
                },
                CallbackUrl = orderDetailsDto.GatewayDto.ResponseReturnedUrl
            };
            return ccdcRequest;
        }
    }
}
