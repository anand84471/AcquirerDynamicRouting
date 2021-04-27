using System;
using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace ExpressCheckoutDb.DBClients.Concrete
{
    public class DBServiceClientHttpEndPointBehaviour : IEndpointBehavior
    {
        private readonly IHttpMessageHandlerFactory _IHttpMessageHandlerFactory;

        public DBServiceClientHttpEndPointBehaviour(IHttpMessageHandlerFactory messageHandlerFactory)
        {
            
            this._IHttpMessageHandlerFactory = messageHandlerFactory;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            Func<HttpClientHandler, HttpMessageHandler> myHandlerFactory = (HttpClientHandler clientHandler) =>
            {
                return _IHttpMessageHandlerFactory.CreateHandler();
            };
          
            bindingParameters.Add(myHandlerFactory);
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
