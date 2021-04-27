using AggExpressCheckoutDBService;
using ExpressCheckoutDb.Entities.Concrete;
using ExpressCheckoutDb.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressCheckoutDb.Repository.Concrete
{
    public class ResponseCodeRepo : IResponseCodeRepo
    {
        private AggregatorExpressCheckoutServiceClient _AggregatorExpressCheckoutServiceClient;

        public ResponseCodeRepo(AggregatorExpressCheckoutServiceClient aggregatorExpressCheckoutServiceClient)
        {
            _AggregatorExpressCheckoutServiceClient = aggregatorExpressCheckoutServiceClient;
        }

        public Task<bool> Delete<ID>(ID pk)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ResponseCodes>> GetAll()
        {
            List <ResponseCodes> responseCodes= new List<ResponseCodes>();

           Dictionary<int,string> dict= await _AggregatorExpressCheckoutServiceClient.GetPinePGCodesAndMessageMappingFromDBAsync();
            if(dict!=null)
            {
                foreach (var key in dict.Keys)
                {
                    string value;
                    dict.TryGetValue(key, out value);
                    responseCodes.Add(new ResponseCodes(key, value));
                }
            }
            return responseCodes;
        }

        public Task<ResponseCodes> GetByID<ID>(ID pk)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResponseCodes>> GetListByID<ID>(ID pk)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Save(ResponseCodes enity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update<ID>(ResponseCodes enitity, ID pk)
        {
            throw new NotImplementedException();
        }
    }
}