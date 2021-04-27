using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ExpressCheckout.BusinessLayer.Abstract
{

    public interface IPayUValidation
    {
        Task<string> CompletePayUTransaction(IFormCollection formCollection);
    }
}
