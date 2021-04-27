using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.ExternalApis.Responses;
using ExpressCheckoutContracts.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ServiceLayer.Abstract
{
   public interface IGlobalBinDataService
    {
        Task<bool> InsertGlobalBindata(DoPaymentRequest doPaymentRequest, long OrderId);
    }
}
