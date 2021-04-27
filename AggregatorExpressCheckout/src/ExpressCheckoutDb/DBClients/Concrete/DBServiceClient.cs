using AggExpressCheckoutDBService;
using ExpressCheckoutDb.DBClients.Abstarct;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.ServiceModel;

namespace ExpressCheckoutDb.DBClients.Concrete
{
    public class DBServiceClient : IDBServiceClient
    {
        private bool disposed = false;
        private IConfiguration configuration;
        private ILogger<DBServiceClient> _Logger;
        private readonly DBServiceClientHttpEndPointBehaviour _DBServiceClientHttpEndPointBehaviourr;
        public DBServiceClient(IConfiguration configuration, DBServiceClientHttpEndPointBehaviour dbServiceClientHttpEndPointBehaviour, ILogger<DBServiceClient> logger)
        {
            this.configuration = configuration;
            this._Logger = logger;
            this._DBServiceClientHttpEndPointBehaviourr = dbServiceClientHttpEndPointBehaviour;
            Initialize();
        }

        ~DBServiceClient()
        {
            Dispose(false);
        }

        private void Initialize()
        {
            try
            {

                string wcfUrl = configuration.GetValue<string>("WCFConfig:url");
                int openTimeout =configuration.GetValue<Int32>("WCFConfig:open_timeout_in_sec");
                int closeTimeout = configuration.GetValue<Int32>("WCFConfig:close_timeout_in_sec");
                int sendTimeout = configuration.GetValue<Int32>("WCFConfig:send_timeout_in_sec");
                int recieveTimeout = configuration.GetValue<Int32>("WCFConfig:receive_timeout_in_sec");
                long maxBuffredPoolSize =configuration.GetValue<Int64>("WCFConfig:max_buffered_pool_size");
                int maxReceivedMessageSize = configuration.GetValue<Int32>("WCFConfig:max_received_msg_size");

                BasicHttpBinding vidiBinding = new BasicHttpBinding();
                vidiBinding.OpenTimeout = new TimeSpan(0, 0, openTimeout);
                vidiBinding.CloseTimeout = new TimeSpan(0, 0, closeTimeout);
                vidiBinding.SendTimeout = new TimeSpan(0, 0, sendTimeout);
                vidiBinding.ReceiveTimeout = new TimeSpan(0, 0, recieveTimeout);
                vidiBinding.MaxBufferPoolSize = maxBuffredPoolSize;  
                vidiBinding.MaxReceivedMessageSize = maxReceivedMessageSize;
                vidiBinding.UseDefaultWebProxy = true;
   
                EndpointAddress myEndPPtAdd = new EndpointAddress(wcfUrl);
                _AggregatorExpressCheckoutServiceClient = new AggregatorExpressCheckoutServiceClient(vidiBinding, myEndPPtAdd);
                _AggregatorExpressCheckoutServiceClient.Endpoint.EndpointBehaviors.Add(_DBServiceClientHttpEndPointBehaviourr);
            }
            catch(Exception ex)
            {
                this._Logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._Logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._Logger.LogError(ex.ToString());

            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {
                    if (_AggregatorExpressCheckoutServiceClient != null)
                    {
                        if (_AggregatorExpressCheckoutServiceClient.State == System.ServiceModel.CommunicationState.Faulted)
                        {
                            _AggregatorExpressCheckoutServiceClient.Abort();
                        }
                        else
                        {
                            _AggregatorExpressCheckoutServiceClient.Close();
                        }
                       
                        _AggregatorExpressCheckoutServiceClient = null;
                    }

                    disposed = true;
                }
            }
           
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public AggregatorExpressCheckoutServiceClient _AggregatorExpressCheckoutServiceClient { get; set; }
    }
}