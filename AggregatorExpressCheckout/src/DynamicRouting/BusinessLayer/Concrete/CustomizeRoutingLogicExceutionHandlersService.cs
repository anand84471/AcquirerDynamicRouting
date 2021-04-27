using Core.Features.ExceptionHandling.Concrete;
using Core.Utilities;
using DynamicRouting.BusinessLayer.Abstract;
using DynamicRouting.JSExecutionEngineSetupInitializer;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.Routing;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests.Routing;
using JavaScriptEngineSwitcher.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicRouting.BusinessLayer.Concrete
{
    public class CustomizeRoutingLogicExceutionHandlersService : IRoutingLogicExceutionHandlersService
    {
        ILogger<CustomizeRoutingLogicExceutionHandlersService> _Logger;
        private readonly IServiceProvider _ServiceProvider;
        public CustomizeRoutingLogicExceutionHandlersService(ILogger<CustomizeRoutingLogicExceutionHandlersService> logger,
            IServiceProvider serviceProvider)
        {
            _Logger = logger;
            _ServiceProvider = serviceProvider;
        }
        public async  Task<List<RoutingWiseDetailsDto>> ExecuteAndFetch(OrderDetailsDto orderDetailsDtoFromDB, RoutingRequest routingRequest, 
            MerchantRoutingConfigDetailsDto merchantRoutingConfigDetailsDto)
        {
          
            //return Task.Run(() =>
            //{
                    List<RoutingWiseDetailsDto> lsRoutingWiseDetailsDtos = new List<RoutingWiseDetailsDto>();
            try
            {

                String logicToBeExecute = GenericUtility.GetJsonStringFromBase64EncodeStriing(merchantRoutingConfigDetailsDto.CustomizedRoutingJSLogicInBase64EncodedForm);
                if (String.IsNullOrEmpty(logicToBeExecute))
                {
                    _Logger.LogInformation("[ExecuteAndFetch]Base64 encode js logic is not present for customized routing for order id:{0} and merchant id:{1} customized routing config id:{2} ",
                        routingRequest.orderRequest.orderId,
                        routingRequest.orderRequest.merchantId, merchantRoutingConfigDetailsDto.CustomizedRoutingConfigId

                    );
                    return await Task.FromResult(lsRoutingWiseDetailsDtos);
                    // return lsRoutingWiseDetailsDtos;
                }
                IJsEngine engine = null;
                try
                {
                    JSExecutionInitializer jSExecutionInitializer = (JSExecutionInitializer)_ServiceProvider.GetService(typeof(JSExecutionInitializer));
                    // JSExecutionInitializer jSExecutionInitializer = new JSExecutionInitializer((ILogger<JSExecutionInitializer>)_ServiceProvider.GetService(typeof(ILogger<JSExecutionInitializer>)));
                     engine = jSExecutionInitializer.GetJsEngine();
                    if (engine == null)
                    {
                        _Logger.LogInformation("[ExecuteAndFetch]Unable to intialize the V8 JS Engine for customized routing for order id:{0} and merchant id:{1} customized routing config id:{2} ",
                           routingRequest.orderRequest.orderId, routingRequest.orderRequest.merchantId, merchantRoutingConfigDetailsDto.CustomizedRoutingConfigId);
                        // return lsRoutingWiseDetailsDtos;
                        return await Task.FromResult(lsRoutingWiseDetailsDtos);
                    }

                    JSLogicRequestBuilder jSLogicRequestBuilder = new JSLogicRequestBuilder(engine, routingRequest, (ILogger<JSLogicRequestBuilder>)_ServiceProvider.GetService(typeof(ILogger<JSLogicRequestBuilder>)));


                    string[] gateways = jSLogicRequestBuilder
                   .SetDataAndPassRoutingLogic(logicToBeExecute)
                   .Execute()
                   .Fetch();

                    List<EnumGateway> enumGateways = gateways.Select(i => (EnumGateway)Enum.Parse(typeof(EnumGateway), i)).ToList();
                    if (enumGateways != null && enumGateways.Count > 0)
                    {
                        lsRoutingWiseDetailsDtos.Add(new RoutingWiseDetailsDto
                        {
                            prefernceScore = merchantRoutingConfigDetailsDto.CustomizedRoutingPerferenceScore,
                            enumGatewaysList = enumGateways

                        });
                    }
                    else
                    {
                        _Logger.LogInformation("[ExecuteAndFetch]Either gateway list is null or empty for customized routing for order id:{0} and merchant id:{1} ", routingRequest.orderRequest.orderId,
                            routingRequest.orderRequest.merchantId
                           );
                    }
                }
                catch (JsExceutionExecption ex)
                {
                    this._Logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                    this._Logger.LogError("Exception occured in method :" + ex.TargetSite);
                    this._Logger.LogError(ex.ToString());
                }
                finally
                {
                    if (engine!=null)
                    {
                        engine.Dispose();

                    }


                }
            }
            catch (Exception ex)
            {
                this._Logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._Logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._Logger.LogError(ex.ToString());

            }
                _Logger.LogInformation("[ExecuteAndFetch]Gatewaylist for customized routing for order id:{0} and merchant id:{1} is:{2} ",
                    routingRequest.orderRequest.orderId, routingRequest.orderRequest.merchantId,
                      JsonConvert.SerializeObject(lsRoutingWiseDetailsDtos));

            return await Task.FromResult(lsRoutingWiseDetailsDtos);
           // return lsRoutingWiseDetailsDtos;


           // });
           
                    


                //var orderDetailsDtoJson = JsonConvert.SerializeObject(orderDetailsDtoFromDB);

                //engine.SetVariableValue("priorties", "");
                //engine.SetVariableValue("currentTimeMillis", 1000037);
                //string dynamicRoutingLogic = GenericUtility.GetJsonStringFromBase64EncodeStriing("");
                //engine.SetVariableValue("request", orderDetailsDtoJson);




                //StringBuilder strbuilderVolumeBased = new StringBuilder();




                //strbuilderVolumeBased.Append("const data = JSON.parse(request);");

                //strbuilderVolumeBased.Append(dynamicRoutingLogic);


                //strbuilderVolumeBased.Append("priorties = priorties.toClrArray();");


                //engine.Execute(strbuilderVolumeBased.ToString());


                //object objectPriorties = engine.GetVariableValue("priorties");
                //string[] arrPriorties = JSExecutionInitializer.GetGatewayArrayFromJSArray(objectPriorties);
                //return arrPriorties;


            

        }
    }

}
