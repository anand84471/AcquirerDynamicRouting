using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressCheckout.Controllers
{
    
    [ApiController]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {

        }

        [Route("/routing/index")]
        [HttpGet]
        public String Get()
        {
            return "OK";
        }
    }
}
