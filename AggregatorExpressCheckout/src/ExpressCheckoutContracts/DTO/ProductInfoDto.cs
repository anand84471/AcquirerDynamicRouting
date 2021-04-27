using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO
{
    public class ProductInfoDto
    {
        public ProductInfoDto()
        {

        }
        public ProductDetailsDto[] productDetails { get; set; }
        public string JsonProductDetails
        {
            get
            {
                return JsonConvert.SerializeObject(productDetails); ;
            }
        }

    }
}