using AggExpressCheckoutDBService;
using AutoMapper;
using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutDb.DBClients.Abstarct;
using Microsoft.Extensions.DependencyInjection;
using ExpressCheckoutDb.Repository.Abstract;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ExpressCheckoutDb.Repository.Concrete
{
    internal class MerchantRepo : IMerchantRepo
    {
        private readonly IServiceProvider serviceProvider;

        private readonly IMapper _mapper;

        private readonly ILogger<MerchantRepo> _logger;

        /// <summary>Initializes a new instance of the <see cref="CustomerRepo"/> class.</summary>
        /// <param name="aggregatorExpressCheckoutServiceClient">The aggregator express checkout service client.</param>
        public MerchantRepo(IServiceProvider serviceProvider, IMapper mapper,
            ILogger<MerchantRepo> _logger)
        {
            this.serviceProvider = serviceProvider;
            _mapper = mapper;
            this._logger = _logger;
        }

        public async Task<MerchantDto> GetMerchantData(int MerchantId)
        {
            MerchantEntity merchantEntity = null;
            try
            {
                using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
                {
                    merchantEntity = await serviceClient._AggregatorExpressCheckoutServiceClient.GetMerchantDataAsync(MerchantId);
                }
                if (merchantEntity == null)
                {
                    throw new InvalidRequestException(ResponseCodeConstants.MERCHANT_DATA_IS_NOT_PRESENT_WITH_MERCHANT);
                }
                return _mapper.Map<MerchantDto>(merchantEntity);
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<bool> IsDuplicateMerchantIDAndMerchantOrderId(int merchantId, string merchantOrderID)
        {
            bool status = false;
            try
            {
                using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
                {
                    status = await serviceClient._AggregatorExpressCheckoutServiceClient.IsMerchantOrderExistsForMerchantAsync(merchantId, merchantOrderID);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return status;
        }



        public async Task<bool> ValidatePaymentMode(int merchantId , int PaymentMode)
        {
            bool status = false;
            try
            {
                using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
                {
                    status = await serviceClient._AggregatorExpressCheckoutServiceClient.ValidatePaymentModeAsync( merchantId, PaymentMode);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return status;
        }
        public async Task<int[]> GetPaymentModesMappedWithMerchant(int merchantId)
        {
            int[] lsPaymentModes = null;


            //AggregatorExpressCheckoutServiceClient objAggregatorExpressCheckoutServiceClient =
            //    this.serviceProvider.GetService<IDBServiceClient>()._AggregatorExpressCheckoutServiceClient;

            try
            {
                using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
                {
                    lsPaymentModes = await serviceClient._AggregatorExpressCheckoutServiceClient.GetPaymentModeEnabledOnMerchantAsync(merchantId);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }

            return lsPaymentModes;
        }

        public async Task<MerchantGatewayConfigurationMappingDto> GetMerchantPaymentGatewayConfigurationDetails(int merchantId,int gatewayId)
        {

            MerchantGatewayConfigurationMappingEntity merchantGatewayConfigurationMappingEntity = null;
            MerchantGatewayConfigurationMappingDto merchantGatewayConfigurationMappingDto = null;

            try
            {
                using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
                {
                    merchantGatewayConfigurationMappingEntity = await serviceClient._AggregatorExpressCheckoutServiceClient.GetMerchantGatewayConfigurationDetailsAsync(merchantId, gatewayId);
                }
                if (merchantGatewayConfigurationMappingEntity != null)
                {
                    merchantGatewayConfigurationMappingDto = _mapper.Map<MerchantGatewayConfigurationMappingDto>(merchantGatewayConfigurationMappingEntity);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }

            return merchantGatewayConfigurationMappingDto;

        }

    }
}