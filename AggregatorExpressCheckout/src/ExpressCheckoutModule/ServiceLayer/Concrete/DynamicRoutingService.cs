using ExpressCheckoutContracts;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ServiceLayer.Concrete
{
    class DynamicRoutingService : IDynamicRoutingService

    {
        private readonly IDynamicRoutingRepo _dynamicRoutingRepo;

        public DynamicRoutingService(IDynamicRoutingRepo dynamicRoutingRepo)   
        {
            _dynamicRoutingRepo = dynamicRoutingRepo;
            
        }
        public async Task<DynamicRoutingDetailsDto> GetDynamicRoutingDetails(int MerchantId)
        {
            return await _dynamicRoutingRepo.GetDynamicRoutingDetails(MerchantId);


        }
    }
}
