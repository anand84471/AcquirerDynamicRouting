using DynamicRouting.BusinessLayer.Abstract;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.Routing;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests.Routing;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutDb.Repository.Concrete;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicRouting.BusinessLayer.Concrete
{
   
    public class SpecialRoutingLogicExecutionHandlerService : IRoutingLogicExceutionHandlersService
    {
        private IDynamicRoutingRepo _DynamicRoutingRepo;
        private ILogger<SpecialRoutingLogicExecutionHandlerService> _Logger;
        public SpecialRoutingLogicExecutionHandlerService(IDynamicRoutingRepo dynamicRoutingRepo, ILogger<SpecialRoutingLogicExecutionHandlerService> logger)
        {
            _DynamicRoutingRepo = dynamicRoutingRepo;
            _Logger = logger;
        }
        public async Task<List<RoutingWiseDetailsDto>> ExecuteAndFetch(OrderDetailsDto orderDetailsDtoFromDB, RoutingRequest routingRequest,
            MerchantRoutingConfigDetailsDto merchantRoutingConfigDetailsDto)
        {
           

                    List<RoutingWiseDetailsDto> lsRoutingWiseDetailsDtos = new List<RoutingWiseDetailsDto>();
                    try
                    {
                        if (merchantRoutingConfigDetailsDto.SpecialRoutingConfigId > 0)
                        {
                            Task<SpecialRoutingDetailsDto[]> taskCardBinRouting = this.GetGatewayAccToCardBinRoutingConfig(merchantRoutingConfigDetailsDto.SpecialRoutingConfigId,
                                routingRequest.paymentRequest.cardBin);

                            Task<SpecialRoutingDetailsDto[]> taskIssuerRouting = this.GetGatewayAccToIssuerRoutingConfig(merchantRoutingConfigDetailsDto.SpecialRoutingConfigId,
                              (int)routingRequest.paymentRequest.cardIssuer.GetValueOrDefault());

                            Task<SpecialRoutingDetailsDto[]> taskCardBrandRouting = this.GetGatewayAccToCardBrandRoutingConfig(merchantRoutingConfigDetailsDto.SpecialRoutingConfigId,
                             (int)routingRequest.paymentRequest.cardBrand.GetValueOrDefault());

                    await Task.WhenAll(taskCardBinRouting, taskIssuerRouting, taskCardBrandRouting);
                    SpecialRoutingDetailsDto[] cardBinRoutingDetails = await taskCardBinRouting;
                            SpecialRoutingDetailsDto[] issuerRoutingDetails = await taskIssuerRouting;
                            SpecialRoutingDetailsDto[] cardBrandRoutingDetails = await taskCardBrandRouting;

                    if (cardBinRoutingDetails != null && cardBinRoutingDetails.Length > 0)
                            {
                                lsRoutingWiseDetailsDtos.AddRange(
                                cardBinRoutingDetails.Select(x =>
                                new RoutingWiseDetailsDto
                                {
                                    enumGatewaysList = new List<EnumGateway> { (EnumGateway)x.GatewayId },
                                    prefernceScore = x.PerfernceScore
                                }).ToList());

                            }
                            else
                            {
                            _Logger.LogInformation("[ExecuteAndFetch]Either gateway list is null or empty for special routing card bin for order id:{0} and merchant id:{1} special routing config id:{2} ", routingRequest.orderRequest.orderId,
                                routingRequest.orderRequest.merchantId,
                                merchantRoutingConfigDetailsDto.SpecialRoutingConfigId
                             );
                             }

                            if (issuerRoutingDetails != null && issuerRoutingDetails.Length > 0)
                            {
                                lsRoutingWiseDetailsDtos.AddRange(
                                issuerRoutingDetails.Select(x =>
                                new RoutingWiseDetailsDto
                                {
                                    enumGatewaysList = new List<EnumGateway> { (EnumGateway)x.GatewayId },
                                    prefernceScore = x.PerfernceScore
                                }).ToList());

                            }
                            else
                            {
                                _Logger.LogInformation("[ExecuteAndFetch]Either gateway list is null or empty for special routing issuer for order id:{0} and merchant id:{1} special routing config id:{2}", routingRequest.orderRequest.orderId, routingRequest.orderRequest.merchantId
                                 , merchantRoutingConfigDetailsDto.SpecialRoutingConfigId);

                            }
                            if (cardBrandRoutingDetails != null && cardBrandRoutingDetails.Length > 0)
                            {
                                lsRoutingWiseDetailsDtos.AddRange(
                                cardBrandRoutingDetails.Select(x =>
                                new RoutingWiseDetailsDto
                                {
                                    enumGatewaysList = new List<EnumGateway> { (EnumGateway)x.GatewayId },
                                    prefernceScore = x.PerfernceScore
                                }).ToList());

                            }
                            else
                            {
                            _Logger.LogInformation("[ExecuteAndFetch]Either gateway list is null or empty for special routing card brand for order id:{0} and merchant id:{1} special routing config id:{2} ", routingRequest.orderRequest.orderId, routingRequest.orderRequest.merchantId
                              , merchantRoutingConfigDetailsDto.SpecialRoutingConfigId  );

                             }



                        }
                       

                    }
                    catch (Exception ex)
                    {
                        this._Logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                        this._Logger.LogError("Exception occured in method :" + ex.TargetSite);
                        this._Logger.LogError(ex.ToString());
                     }
            _Logger.LogInformation("[ExecuteAndFetch]Gatewaylist for special routing for order id:{0} and merchant id:{1} is:{2} :special routing config id:{3} ", routingRequest.orderRequest.orderId, routingRequest.orderRequest.merchantId,
                     JsonConvert.SerializeObject(lsRoutingWiseDetailsDtos), merchantRoutingConfigDetailsDto.SpecialRoutingConfigId);
            return lsRoutingWiseDetailsDtos;

               
           
        }

        private async Task<SpecialRoutingDetailsDto[]> GetGatewayAccToCardBinRoutingConfig(long configId, string cardBin)
        {
            return await _DynamicRoutingRepo.GetGatewayAccToCardBinRoutingConfig(configId, cardBin);
        }
        private async Task<SpecialRoutingDetailsDto[]> GetGatewayAccToIssuerRoutingConfig(long configId, int issuerId)
        {
            return await _DynamicRoutingRepo.GetGatewayAccToIssuerRoutingConfig(configId, issuerId);
        }
        private async Task<SpecialRoutingDetailsDto[]> GetGatewayAccToCardBrandRoutingConfig(long configId, int assosicationTypeId)
        {
            return await _DynamicRoutingRepo.GetGatewayAccToCardBrandRoutingConfig(configId, assosicationTypeId);
        }
    }
}
