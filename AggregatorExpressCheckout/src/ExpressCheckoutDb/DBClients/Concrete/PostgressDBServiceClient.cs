using ExpressCheckoutDb.DBClients.Abstract;
using ExpressCheckoutDb.Postgress.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutDb.DBClients.Concrete
{
    class PostgressDBServiceClient : IPostgressDBServiceClient
    {
        private bool disposed = false;
        private IConfiguration configuration;

        public TorrentPayDBService _EdgeXCeedDBService { get; set; }
        private ILogger<TorrentPayDBService> _logger;
        public PostgressDBServiceClient(IConfiguration configuration,ILogger<TorrentPayDBService> logger)
        {
            this.configuration = configuration;
            this._logger = logger;
            Initialize();
        }
        private void Initialize()
        {
            try
            {
                _EdgeXCeedDBService = new TorrentPayDBService(this._logger,this.configuration);
            }
            catch (Exception ex)
            {

            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (_EdgeXCeedDBService != null)
                    {

                        _EdgeXCeedDBService = null;
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

    }
}
