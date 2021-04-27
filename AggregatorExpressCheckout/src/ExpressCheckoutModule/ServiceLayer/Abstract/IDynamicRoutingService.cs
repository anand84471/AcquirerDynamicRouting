using ExpressCheckoutContracts;
using ExpressCheckoutContracts.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ServiceLayer.Abstract
{
   public interface IDynamicRoutingService
    {
        Task<DynamicRoutingDetailsDto> GetDynamicRoutingDetails(int MerchantId);
    }
}
