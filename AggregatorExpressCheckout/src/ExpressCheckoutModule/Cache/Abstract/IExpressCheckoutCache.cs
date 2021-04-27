using ExpressCheckoutContracts.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.Cache.Abstract
{
    public interface IExpressCheckoutCache
    {
        Dictionary<int, string> ResponseCodes { get; set; }

        Task IntializeCache();

        String GetResponseMsg(int responseCode);

        void AddOrderDetailsRequest(long orderId, OrderDetailsDto orderDetailsRequest);

        OrderDetailsDto GetOrderDetailsRequest(long orderId);
    }
}