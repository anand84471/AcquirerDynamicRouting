using DynamicRouting.BusinessLayer.Abstract;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.Routing;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicRouting.BusinessLayer.Concrete
{
    public class SimpleRoutingLogicExceutionHandlersService : IRoutingLogicExceutionHandlersService
    {
        ILogger<SimpleRoutingLogicExceutionHandlersService> _Logger;
        public SimpleRoutingLogicExceutionHandlersService(ILogger<SimpleRoutingLogicExceutionHandlersService> logger)
        {
            _Logger = logger;
        }
        public async Task<List<RoutingWiseDetailsDto>> ExecuteAndFetch(OrderDetailsDto orderDetailsDtoFromDB, RoutingRequest routingRequest, 
            MerchantRoutingConfigDetailsDto merchantRoutingConfigDetailsDto)
        {
           
           //return Task.Run(() =>
           // {
                List<RoutingWiseDetailsDto> lsRoutingWiseDetailsDtos = new List<RoutingWiseDetailsDto>();
                try
                {
                    if(String.IsNullOrEmpty(merchantRoutingConfigDetailsDto.SimpleRoutingLogicCSV))
                    {
                        _Logger.LogInformation("[ExecuteAndFetch]SimpleRoutingLogicCSV logic is not present for simple routing for order id:{0} and merchant id:{1} simple routing config id:{2} ",
                              routingRequest.orderRequest.orderId, routingRequest.orderRequest.merchantId, merchantRoutingConfigDetailsDto.SimpleRoutingConfigId);
                    return await Task.FromResult(lsRoutingWiseDetailsDtos);
                  }
                    List<EnumGateway> enumGateways = merchantRoutingConfigDetailsDto.SimpleRoutingLogicCSV.Split(',').Select(i => (EnumGateway)Enum.Parse(typeof(EnumGateway), i,true)).ToList();
                    if (enumGateways != null && enumGateways.Count > 0)
                    {
                        lsRoutingWiseDetailsDtos.Add(new RoutingWiseDetailsDto
                        {
                            prefernceScore = merchantRoutingConfigDetailsDto.SimpleRoutingPerferenceScore,
                            enumGatewaysList = enumGateways

                        });
                    }
                    else
                    {
                        _Logger.LogInformation("[ExecuteAndFetch]Either gateway list is null or empty for simple routing for order id:{0} and merchant id:{1} ", routingRequest.orderRequest.orderId, routingRequest.orderRequest.merchantId
                         );
                    }


                }
                catch (Exception ex)
                {
                    this._Logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                    this._Logger.LogError("Exception occured in method :" + ex.TargetSite);
                    this._Logger.LogError(ex.ToString());
                }
                _Logger.LogInformation("[ExecuteAndFetch]Gatewaylist for simple routing for order id:{0} and merchant id:{1} is:{2} ", routingRequest.orderRequest.orderId, routingRequest.orderRequest.merchantId,
                       JsonConvert.SerializeObject(lsRoutingWiseDetailsDtos)  );
            return await Task.FromResult(lsRoutingWiseDetailsDtos);

            // });




        }
    }
}
