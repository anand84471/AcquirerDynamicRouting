using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using Core.Utilities;
using ExpressCheckoutContracts.DTO.Routing.DtoExposeToMerchant;
using ExpressCheckoutContracts.Requests.Routing;
using JavaScriptEngineSwitcher.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicRouting.JSExecutionEngineSetupInitializer
{
    public class JSLogicRequestBuilder
    {
        private StringBuilder _JSLogicStringBuilder = null;
        private IJsEngine _JSEngine = null;
        private RoutingRequest _RoutingRequest = null;
        private ILogger<JSLogicRequestBuilder> _Logger;
        public JSLogicRequestBuilder(IJsEngine jsEngine, RoutingRequest routingRequest, ILogger<JSLogicRequestBuilder> logger)
        {
            _JSLogicStringBuilder = new StringBuilder();
            _JSEngine = jsEngine;
            _RoutingRequest = routingRequest;
            _Logger = logger;
            SetJsEngineVarriables();
        }

        private void SetJsEngineVarriables()
        {
            _JSEngine.SetVariableValue("priorties", "");
            
        }
        internal JSLogicRequestBuilder SetDataAndPassRoutingLogic(string logicToBeExecute)
        {
           return this.SetOrderData().SetPaymentData().SetTxnData().SeAdditionalData().SetLogic(logicToBeExecute);
        }

        internal JSLogicRequestBuilder Execute()
        {
            _JSLogicStringBuilder.Append("priorties = priorties.toClrArray();");
            try
            {
                _JSEngine.Execute(_JSLogicStringBuilder.ToString());

            }
            catch(Exception ex)
            {
                this._Logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._Logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._Logger.LogError(ex.ToString());
                throw new JsExceutionExecption(ResponseCodeConstants.JS_EXCEUTION_FAILS);
            }
           
            return this;
        }
        internal string[] Fetch()
        {
            object objectPriorties = _JSEngine.GetVariableValue("priorties");
            string[] arrPriorties = JSExecutionInitializer.GetGatewayArrayFromJSArray(objectPriorties);
            return arrPriorties;
        }


        private JSLogicRequestBuilder SetLogic(String logicToBeExecute)
        {
           _JSLogicStringBuilder.Append(logicToBeExecute);
            return this;
           
        }
        private JSLogicRequestBuilder SetOrderData()
        {
            OrderDto orderDto = CreateOrderDto(_RoutingRequest);
            _JSEngine.SetVariableValue("orderData", JsonConvert.SerializeObject(orderDto));
            _JSLogicStringBuilder.Append("const order = JSON.parse(orderData);");
            return this;
        }
        private JSLogicRequestBuilder SetPaymentData()
        {
            PaymentDto paymentDto= CreatePaymentDto(_RoutingRequest);
            _JSEngine.SetVariableValue("paymentData", JsonConvert.SerializeObject(paymentDto));
            _JSLogicStringBuilder.Append("const payment = JSON.parse(paymentData);");
            return this;

        }

       

        private JSLogicRequestBuilder SetTxnData()
        {
            TxnDto txnDto = CreateTxnDto(_RoutingRequest);
            _JSEngine.SetVariableValue("txnData", JsonConvert.SerializeObject(txnDto));
            _JSLogicStringBuilder.Append("const txn = JSON.parse(txnData);");
            return this;

        }

        private JSLogicRequestBuilder SeAdditionalData()
        {
            int unixTimeStamp = _RoutingRequest.additionalDataRequest.unxiTimeStamp;
            if(unixTimeStamp<=0)
            {
                unixTimeStamp = GenericUtility.GetCurrentUnixTimeStamp();
            }

            _JSEngine.SetVariableValue("unixtimestamp", unixTimeStamp);
            _JSLogicStringBuilder.Append("const timestamp = unixtimestamp;");
            return this;

        }
        private OrderDto CreateOrderDto(RoutingRequest routingRequest)
        {
            OrderDto orderDto = new OrderDto();
            orderDto.orderId = routingRequest.orderRequest.orderId;
            orderDto.amount = routingRequest.orderRequest.amount;
            orderDto.currency = routingRequest.orderRequest.currency;
            orderDto.preferredGateway = routingRequest.orderRequest.preferredGateway.GetValueOrDefault();
            orderDto.UDF1 = String.IsNullOrEmpty(routingRequest.orderRequest.UDF1) ? String.Empty : routingRequest.orderRequest.UDF1;
            orderDto.UDF2 = String.IsNullOrEmpty(routingRequest.orderRequest.UDF2) ? String.Empty : routingRequest.orderRequest.UDF2;
            orderDto.UDF3 = String.IsNullOrEmpty(routingRequest.orderRequest.UDF3) ? String.Empty : routingRequest.orderRequest.UDF3;
            orderDto.UDF4 = String.IsNullOrEmpty(routingRequest.orderRequest.UDF4) ? String.Empty : routingRequest.orderRequest.UDF4;
            orderDto.UDF5 = String.IsNullOrEmpty(routingRequest.orderRequest.UDF5) ? String.Empty : routingRequest.orderRequest.UDF5;
            orderDto.UDF6 = String.IsNullOrEmpty(routingRequest.orderRequest.UDF6) ? String.Empty : routingRequest.orderRequest.UDF6;
            orderDto.UDF7 = String.IsNullOrEmpty(routingRequest.orderRequest.UDF7) ? String.Empty : routingRequest.orderRequest.UDF7;
            orderDto.UDF8 = String.IsNullOrEmpty(routingRequest.orderRequest.UDF8) ? String.Empty : routingRequest.orderRequest.UDF8;
            orderDto.UDF9 = String.IsNullOrEmpty(routingRequest.orderRequest.UDF9) ? String.Empty : routingRequest.orderRequest.UDF9;
            orderDto.UDF10 = String.IsNullOrEmpty(routingRequest.orderRequest.UDF10) ? String.Empty : routingRequest.orderRequest.UDF10;



            return orderDto;
        }

        private TxnDto CreateTxnDto(RoutingRequest routingRequest) 
        {
            TxnDto txnDto = new TxnDto();
            txnDto.txnId = routingRequest.txnRequest.txnId;
            txnDto.paymentode = routingRequest.txnRequest.paymentode.GetValueOrDefault(); ;
            return txnDto;

        }

        private PaymentDto CreatePaymentDto(RoutingRequest routingRequest)
        {
            PaymentDto paymentDto = new PaymentDto();
            paymentDto.cardBin = String.IsNullOrEmpty(routingRequest.paymentRequest.cardBin) ? String.Empty : routingRequest.paymentRequest.cardBin;
            paymentDto.cardBrand = routingRequest.paymentRequest.cardBrand.GetValueOrDefault(); ;
            paymentDto.cardIssuer = routingRequest.paymentRequest.cardIssuer.GetValueOrDefault(); ;
            paymentDto.cardType = routingRequest.paymentRequest.cardType.GetValueOrDefault(); ;
            paymentDto.paymentMethod = String.IsNullOrEmpty(routingRequest.paymentRequest.paymentMethod) ? String.Empty : routingRequest.paymentRequest.paymentMethod; 
            paymentDto.paymentMethodType = String.IsNullOrEmpty(routingRequest.paymentRequest.paymentMethodType) ? String.Empty : routingRequest.paymentRequest.paymentMethodType;
            return paymentDto;

        }

      
      
    }
}
