using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Cache.Abstract;
using Core.Constants;
using DynamicRouting.BusinessLayer.Abstract;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Requests.Routing;
using ExpressCheckoutContracts.Response.Abstract;
using ExpressCheckoutContracts.Response.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamicRouting.Controllers
{
    [Route("api/Dynamic")]
    [ApiController]
    public class RoutingEngineController : ControllerBase
    {
        //private readonly ICoreCache _coreCache;
        private readonly IRoutingEngineService _DynamicRoutingService;
        /// <summary>
        /// Customer Api Constructor
        /// </summary>
        /// <param name="dynamicRoutingCustomerValidation">Gate business layer for validations</param>
        public RoutingEngineController(ICoreCache coreCache, IRoutingEngineService routingEngineService)
        {
            //_coreCache = coreCache;
            _DynamicRoutingService = routingEngineService;
        }

        [Route("routing")]
        public async Task<MasterResponse> GetGateways(RoutingRequest routingRequest)
        {
            List<string> listGatewayOrder;
           
                listGatewayOrder = await _DynamicRoutingService.GetGateways(routingRequest);
                return new DynamicRoutingGatewayResponse
                {
                    Code = ResponseCodeConstants.SUCCESS,
                    Message = "SUCCESS",
                    Priorties = listGatewayOrder
                };
            
           
        }
    }
}