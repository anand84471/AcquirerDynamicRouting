using AspNetCore.AsyncInitialization;
using ExpressCheckoutModule.Cache.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace TorrentPay.AsyncIntializiers
{
    public class AsyncCacheInitializer:IAsyncInitializer
    {
        private IExpressCheckoutCache _CoreCache;
        public AsyncCacheInitializer(IExpressCheckoutCache CoreCache)
        {
            _CoreCache = CoreCache;
        }

        public async Task InitializeAsync()
        {
            await _CoreCache.IntializeCache();
        }
    }
}
