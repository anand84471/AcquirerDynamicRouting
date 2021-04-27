using Core.Cache.Abstract;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutModule.Cache.Abstract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.Cache.Concrete
{
    public class ExpressCheckoutCache : IExpressCheckoutCache
    {
        private readonly IResponseCodeRepo responseCodeRepo;
        private ConcurrentDictionary<long, OrderDetailsDto> orderDetailsDictionary;
        private ICoreCache coreCache;

        public Dictionary<int, string> ResponseCodes { get; set; }

        public ExpressCheckoutCache(IResponseCodeRepo responseCodeRepo, ICoreCache coreCache )
        {
            this.responseCodeRepo = responseCodeRepo;
            orderDetailsDictionary = new ConcurrentDictionary<long, OrderDetailsDto>();
            this.coreCache = coreCache;
        }

        public async Task IntializeCache()
        {
           
            await this.InitializeResponseCodesDictionaryAsync();
        }

        private async Task InitializeResponseCodesDictionaryAsync()
        {
            var responseCodes = await this.responseCodeRepo.GetAll();
            ResponseCodes = new Dictionary<int, string>();
            foreach (var responseCode in responseCodes)
            {
                ResponseCodes.Add(responseCode.Code, responseCode.Message);
            }
            this.coreCache.ResponseCodes = ResponseCodes;


        }

        public string GetResponseMsg(int responseCode)
        {
            string responsMsg = String.Empty;
            if (ResponseCodes.ContainsKey(responseCode))
            {
                responsMsg = ResponseCodes[responseCode];
            }
            return responsMsg;
        }

        public OrderDetailsDto GetOrderDetailsRequest(long orderId) 
        {
            if (orderDetailsDictionary.ContainsKey(orderId)) 
            {
                return orderDetailsDictionary[orderId];
            }
            throw new Exception();
        }

        public void AddOrderDetailsRequest(long orderId, OrderDetailsDto orderDetailsDtO) 
        {
            orderDetailsDictionary.TryAdd(orderId, orderDetailsDtO);
        }
    }
}