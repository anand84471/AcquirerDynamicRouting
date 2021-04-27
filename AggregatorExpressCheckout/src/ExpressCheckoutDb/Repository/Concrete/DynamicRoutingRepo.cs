using AggExpressCheckoutDBService;
using AutoMapper;
using ExpressCheckoutDb.DBClients.Abstarct;
using ExpressCheckoutDb.Repository.Abstract;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ExpressCheckoutContracts;
using ExpressCheckoutContracts.DTO.Routing;
using Microsoft.Extensions.Logging;

namespace ExpressCheckoutDb.Repository.Concrete
{
   public class DynamicRoutingRepo :IDynamicRoutingRepo
    {
        private readonly IMapper _mapper;

        private readonly IServiceProvider _serviceProvider;

        private ILogger<DynamicRoutingRepo> _Logger;

        /// <summary>Initializes a new instance of the <see cref="CustomerRepo"/> class.</summary>
        /// <param name="aggregatorExpressCheckoutServiceClient">The aggregator express checkout service client.</param>
        public DynamicRoutingRepo(IMapper mapper, IServiceProvider serviceProvider, ILogger<DynamicRoutingRepo> logger)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _Logger=logger;
    }

        public async Task<DynamicRoutingDetailsDto> GetDynamicRoutingDetails(int MerchantId)
        {
            DynamicRoutingDetailsDto dynamicRoutingDetailsDto = null;
            DynamicRotuingDetailsEntity dynamicRotuingDetailsEntity = null;
            using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
            {
                dynamicRotuingDetailsEntity = await serviceClient._AggregatorExpressCheckoutServiceClient.GetDynamicRoutingDetailsAsync(MerchantId);
            }
            if (dynamicRotuingDetailsEntity != null)
            {
                dynamicRoutingDetailsDto = _mapper.Map<DynamicRoutingDetailsDto>(dynamicRotuingDetailsEntity);
            }

            return dynamicRoutingDetailsDto;
        }

        public async Task<MerchantRoutingConfigDetailsDto> GetRoutingCOnfigurationDetails(int MerchantId)
        {
            MerchantRoutingConfigDetailsDto merchantRoutingConfigDetailsDto = null;
            try
            {
                
                MerchantRoutingConfigDetailsEntity merchantRoutingConfigDetailsEntity = null;
                using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
                {
                    merchantRoutingConfigDetailsEntity = await serviceClient._AggregatorExpressCheckoutServiceClient.GetRoutingCOnfigurationDetailsAsync(MerchantId);
                }
            if (merchantRoutingConfigDetailsEntity != null)
                {
                    merchantRoutingConfigDetailsDto = _mapper.Map<MerchantRoutingConfigDetailsDto>(merchantRoutingConfigDetailsEntity);
                }
            }
            catch (Exception ex)
            {
                this._Logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._Logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._Logger.LogError(ex.ToString());
            }
            return merchantRoutingConfigDetailsDto;
        }

        public async Task<SpecialRoutingDetailsDto[]> GetGatewayAccToCardBinRoutingConfig(long configId,string cardBin)
        {
            SpecialRoutingDetailsDto[] specialRoutingDetailsDto = null;
            try
            {
                SpecialRoutingDetailsEntity[] specialRoutingDetailsEntity = null;
                using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
                {
                    specialRoutingDetailsEntity = await serviceClient._AggregatorExpressCheckoutServiceClient.GetGatewayAccToCardBinRoutingConfigAsync(configId, cardBin);
                }
                if (specialRoutingDetailsEntity != null)
                {
                    specialRoutingDetailsDto = _mapper.Map<SpecialRoutingDetailsDto[]>(specialRoutingDetailsEntity);
                }
            }
            catch (Exception ex)
            {
                this._Logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._Logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._Logger.LogError(ex.ToString());
            }
            return specialRoutingDetailsDto;
        }

        public async Task<SpecialRoutingDetailsDto[]> GetGatewayAccToIssuerRoutingConfig(long configId, int issuerId)
        {
            SpecialRoutingDetailsDto[] specialRoutingDetailsDto = null;
            try { 

             
                SpecialRoutingDetailsEntity[] specialRoutingDetailsEntity = null;
                using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
                {
                    specialRoutingDetailsEntity = await serviceClient._AggregatorExpressCheckoutServiceClient.GetGatewayAccToIssuerRoutingConfigAsync(configId, issuerId);
                }
                if (specialRoutingDetailsEntity != null)
                {
                    specialRoutingDetailsDto = _mapper.Map<SpecialRoutingDetailsDto[]>(specialRoutingDetailsEntity);
                }
              }
             catch (Exception ex)
            {
                this._Logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._Logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._Logger.LogError(ex.ToString());
            }
            return specialRoutingDetailsDto;
        }


        public async Task<SpecialRoutingDetailsDto[]> GetGatewayAccToCardBrandRoutingConfig(long configId, int assosiactionTypeId)
        {
            SpecialRoutingDetailsDto[] specialRoutingDetailsDto = null;
            try { 

                    SpecialRoutingDetailsEntity[] specialRoutingDetailsEntity = null;
                    using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
                    {
                        specialRoutingDetailsEntity = await serviceClient._AggregatorExpressCheckoutServiceClient.GetGatewayAccToCardBrandRoutingConfigAsync(configId, assosiactionTypeId);
                    }
                    if (specialRoutingDetailsEntity != null)
                    {
                        specialRoutingDetailsDto = _mapper.Map<SpecialRoutingDetailsDto[]>(specialRoutingDetailsEntity);
                    }
             }
             catch (Exception ex)
            {
                this._Logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._Logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._Logger.LogError(ex.ToString());
            }
            return specialRoutingDetailsDto;
        }
    }
}
