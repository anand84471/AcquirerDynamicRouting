using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO
{
    public class ProductDetailsDto
    {
        public ProductDetailsDto()
        {

        }
        public string ProductCode { get; set; }

        public long ProductAmount { get; set; }
    }
}