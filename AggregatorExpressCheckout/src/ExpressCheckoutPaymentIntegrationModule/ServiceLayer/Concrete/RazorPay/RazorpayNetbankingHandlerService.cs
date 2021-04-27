using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using ExpressCheckoutContracts.Constants;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.Razorpay;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ApiClients.Abstract;
using ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Abstract;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ExpressCheckoutPaymentIntegrationModule.ServiceLayer.Concrete.RazorPay
{
    public class RazorpayNetbankingHandlerService : IRazorpayPaymentModeHandlers
    {
        private readonly IPaymentIntegrationApiClient apiClient;
        private readonly INetbankingRepo netbankingRepo;
        private readonly IOrderRepo orderRepo;

        public RazorpayNetbankingHandlerService(IPaymentIntegrationApiClient apiClient, INetbankingRepo netbankingRepo, IOrderRepo orderRepo)
        {
            this.apiClient = apiClient;
            this.netbankingRepo = netbankingRepo;
            this.orderRepo = orderRepo;
        }

        public async Task<string> DoPayment(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetailsDto, RazorpayOrderResponseDto razorpayOrderResponseDto)
        {
            var netbankingRequest = await CreateNetbankingRequest(doPaymentRequest, orderDetailsDto, razorpayOrderResponseDto);
            var response = await this.apiClient.DoPaymentAtRazorpay(netbankingRequest, orderDetailsDto.MerchantGatewayConfigurationMappingDto, orderDetailsDto.GatewayDto.NetbankingPaymentUrl);
            if (response == null)
            {
                throw new OrderException(ResponseCodeConstants.PAYMENT_FAILED_AT_AGGREGATOR);
            }

            var orderStepDto = new OrderStepDto
            {
                PaymentId = orderDetailsDto.PaymentDataDto.PaymentId,
                RequestSendToGateway = JsonConvert.SerializeObject(netbankingRequest),
                RequestType = 3,
                ResponseReceivedFromGateway = null,
            };

            await this.orderRepo.InsertOrderPaymentStepDetails(orderStepDto);
            return response;
        }

        private async Task<RazorpayNetbankingRequestDto> CreateNetbankingRequest(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetailsDto, RazorpayOrderResponseDto razorpayOrderResponseDto)
        {
            var paymentOptionCode = await this.netbankingRepo.GetNetbankingPaymentOptionCode(EnumGateway.RAZOR_PAY, doPaymentRequest.NetbankingRequest.PaymentCode);
            var netbankinRequest = new RazorpayNetbankingRequestDto
            {
                Amount = orderDetailsDto.OrderTxnInfoDto.Amount,
                Bank = paymentOptionCode,
                OrderId = razorpayOrderResponseDto.Id,
                Currency = razorpayOrderResponseDto.Currency,
                Method = CommonConstants.RAZORPAY_NETBANKING_METHOD,
                Contact = orderDetailsDto.CustomerDto.MobileNumber,
                Email = orderDetailsDto.CustomerDto.EmailId,
                CallbackUrl = orderDetailsDto.GatewayDto.ResponseReturnedUrl
            };
            return netbankinRequest;

        }
    }
}
