using AutoMapper;
using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using Core.Utilities;
using DynamicRouting.BusinessLayer.Abstract;
using DynamicRouting.JSExecutionEngineSetupInitializer;
using ExpressCheckoutContracts;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.Routing;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Requests.Routing;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutModule.Cache.Abstract;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.Jint;
using JavaScriptEngineSwitcher.Msie;
using JavaScriptEngineSwitcher.Node;
using JavaScriptEngineSwitcher.V8;
using JavaScriptEngineSwitcher.Vroom;
using Microsoft.ClearScript;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DynamicRouting.BusinessLayer.Concrete
{
    public class RoutingEngineService : IRoutingEngineService
    {
        private readonly IMapper _mapper;
        private IOrderService _orderService;
        private IExpressCheckoutCache _coreCache;
        private IDynamicRoutingRepo _DynamicRoutingRepo;
        private IRoutingGatewaySortingStartegy _RoutingGatewaySortingStartegy;
        private readonly ILogger<RoutingEngineService> _Logger;
        private readonly IServiceProvider _ServiceProvider;

        public RoutingEngineService(IMapper mapper, IOrderService orderService, IDynamicRoutingService dynamicRoutingService, IExpressCheckoutCache coreCache,
            IDynamicRoutingRepo dynamicRoutingRepo, IRoutingGatewaySortingStartegy routingGatewaySortingStartegy,
            ILogger<RoutingEngineService> logger, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _orderService = orderService;
            _coreCache = coreCache;
            _DynamicRoutingRepo = dynamicRoutingRepo;
            _RoutingGatewaySortingStartegy = routingGatewaySortingStartegy;
            _Logger = logger;
            _ServiceProvider = serviceProvider;
        }
        public async Task<List<string>> GetGateways(RoutingRequest routingRequest)
        {

            if (routingRequest == null || routingRequest.orderRequest == null || (routingRequest.orderRequest.orderId <= 0 && routingRequest.orderRequest.merchantId <= 0))
            {
                _Logger.LogInformation("[GetGateways]Either routing request is null or empty or order id is not valid");
                throw new OrderException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }
            _Logger.LogInformation("[GetGateways]Routing request:{0}", JsonConvert.SerializeObject(routingRequest));
            OrderDetailsDto orderDetailsDtoFromDB = null;
            if (routingRequest.orderRequest.orderId > 0)
            {
                orderDetailsDtoFromDB = await _orderService.GetOrderDetails(routingRequest.orderRequest.orderId);
            }

            else if (routingRequest.orderRequest.merchantId > 0)
            {
                orderDetailsDtoFromDB = new OrderDetailsDto();
                orderDetailsDtoFromDB.MerchantDto = new MerchantDto();
                orderDetailsDtoFromDB.MerchantDto.MerchantId = routingRequest.orderRequest.merchantId;
            }
         
            if (orderDetailsDtoFromDB == null)
            {
                _Logger.LogInformation("[GetGateways]Order not found in DB");
                throw new RoutingException(ResponseCodeConstants.ORDER_DATA_IS_NOT_VALID);
            }
            _Logger.LogInformation("[GetGateways]Order details for merchant id:{0}", orderDetailsDtoFromDB.MerchantDto.MerchantId);

            return await ExecuteDynamicArrayLogic(orderDetailsDtoFromDB, routingRequest);

        }

        private async Task<List<string>> ExecuteDynamicArrayLogic(OrderDetailsDto orderDetailsDtoFromDB, RoutingRequest routingRequest)
        {


            MerchantRoutingConfigDetailsDto merchantRoutingConfigDetailsDto = await this.GetRoutingCOnfigurationDetails(orderDetailsDtoFromDB.MerchantDto.MerchantId);
            if (merchantRoutingConfigDetailsDto == null)
            {
                _Logger.LogInformation("[GetGateways]Merchant routing configuration details not found for merchant id:{0}", orderDetailsDtoFromDB.MerchantDto.MerchantId);
                throw new RoutingException(ResponseCodeConstants.EITHER_MERCHANT_ROUTING_CONFIGURATION_NOT_FOUND_OR_DB_ERROR);
            }

            _Logger.LogInformation("[GetGateways]ExecuteDynamicArrayLogic for merchant id:{0}", orderDetailsDtoFromDB.MerchantDto.MerchantId);

            IRoutingLogicExceutionHandlersService simpleRoutingLogicExceutionHandlersService = (IRoutingLogicExceutionHandlersService)_ServiceProvider.GetService(typeof(SimpleRoutingLogicExceutionHandlersService));
            IRoutingLogicExceutionHandlersService customizeRoutingLogicExceutionHandlersService = (IRoutingLogicExceutionHandlersService)_ServiceProvider.GetService(typeof(CustomizeRoutingLogicExceutionHandlersService));
            IRoutingLogicExceutionHandlersService specialRoutingLogicExceutionHandlersService = (IRoutingLogicExceutionHandlersService)_ServiceProvider.GetService(typeof(SpecialRoutingLogicExecutionHandlerService));


            Task<List<RoutingWiseDetailsDto>> taskSpecialRoutingWiseDetailsDto = specialRoutingLogicExceutionHandlersService.ExecuteAndFetch(orderDetailsDtoFromDB, routingRequest, merchantRoutingConfigDetailsDto);
            Task<List<RoutingWiseDetailsDto>> taskCustomizeRoutingWiseDetailsDto = customizeRoutingLogicExceutionHandlersService.ExecuteAndFetch(orderDetailsDtoFromDB, routingRequest, merchantRoutingConfigDetailsDto);
            Task<List<RoutingWiseDetailsDto>> taskSimpleRoutingWiseDetailsDto = simpleRoutingLogicExceutionHandlersService.ExecuteAndFetch(orderDetailsDtoFromDB, routingRequest, merchantRoutingConfigDetailsDto);

            await Task.WhenAll(taskSpecialRoutingWiseDetailsDto, taskCustomizeRoutingWiseDetailsDto, taskSimpleRoutingWiseDetailsDto);

            List<RoutingWiseDetailsDto> specialRoutingWiseDetailsDto = await taskSpecialRoutingWiseDetailsDto;
            List<RoutingWiseDetailsDto> customizeRoutingWiseDetailsDto = await taskCustomizeRoutingWiseDetailsDto;
            List<RoutingWiseDetailsDto> simpleRoutingWiseDetailsDto = await taskSimpleRoutingWiseDetailsDto;


            List<EnumGateway> enumGateways = _RoutingGatewaySortingStartegy.Sort(simpleRoutingWiseDetailsDto, customizeRoutingWiseDetailsDto, specialRoutingWiseDetailsDto);
            List<string> finalGateways = new List<string>();

            enumGateways.ForEach
                (
                x =>
                {
                    string gateway = x.ToString();
                    if (!finalGateways.Contains(gateway))
                    {
                        finalGateways.Add(gateway);

                    }
                }

                );

            _Logger.LogInformation("[GetGateways]Final gateway list for order id:{0} and merchant id:{1} is:{2}", routingRequest.orderRequest.orderId, orderDetailsDtoFromDB.MerchantDto.MerchantId,
               JsonConvert.SerializeObject(finalGateways));
            return finalGateways;




        }

        public async Task<MerchantRoutingConfigDetailsDto> GetRoutingCOnfigurationDetails(int MerchantId)
        {
            return await _DynamicRoutingRepo.GetRoutingCOnfigurationDetails(MerchantId);

        }



    }
}
