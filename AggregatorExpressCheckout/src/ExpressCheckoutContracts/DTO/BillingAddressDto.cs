using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO
{
    public class BillingAddressDto
    {
        public BillingAddressDto()
        {

        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string AddressPinCode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }
    }
}