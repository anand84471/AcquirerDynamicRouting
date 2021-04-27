using AutoMapper;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutDb.DBClients.Abstarct;
using ExpressCheckoutDb.Repository.Abstract;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using AggExpressCheckoutDBService;

namespace ExpressCheckoutDb.Repository.Concrete
{
    public class GatewayRepo : IGatewayRepo
    {
        private readonly IMapper mapper;

        private readonly IServiceProvider serviceProvider;

        /// <summary>Initializes a new instance of the <see cref="CustomerRepo"/> class.</summary>
        /// <param name="aggregatorExpressCheckoutServiceClient">The aggregator express checkout service client.</param>
        public GatewayRepo(IMapper mapper, IServiceProvider serviceProvider)
        {
            this.mapper = mapper;
            this.serviceProvider = serviceProvider;
        }

        public async Task<GatewayDto> GetGatewayDetails(EnumGateway enumGateway)
        {

            GatewayEntity gatewayEntity = null;
            GatewayDto gatewayDto = null;
            using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
            {
                gatewayEntity = await serviceClient._AggregatorExpressCheckoutServiceClient.GetGatewayDetailsAsync((int)enumGateway);
            }
            if (gatewayEntity != null)
            {
                gatewayDto = mapper.Map<GatewayDto>(gatewayEntity);

            }

            return gatewayDto;

        }
        public async Task<EnableGatewayPaymentModeDto> GetEnableGatewayList(int merchantId,int paymentId)
        {

            EnableGatewayListEntity enableGatewayListEntity = null;
            EnableGatewayPaymentModeDto enableGatewayPaymentModeDto = null;
            using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
            {
                enableGatewayListEntity = await serviceClient._AggregatorExpressCheckoutServiceClient.GetEnableGatewayListAsync(merchantId,paymentId);
            }
            if (enableGatewayListEntity != null)
            {
                enableGatewayPaymentModeDto = mapper.Map<EnableGatewayPaymentModeDto>(enableGatewayListEntity);

            }

            return enableGatewayPaymentModeDto;

        }
    }
}
