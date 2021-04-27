using AspNetCore.AsyncInitialization;
using ExpressCheckoutModule.Cache.Abstract;
using System.Threading.Tasks;

namespace DynamicRouting.AsyncIntializiers
{
    public class AsyncCacheInitializer : IAsyncInitializer
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