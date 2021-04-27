using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts
{
  public class DynamicRoutingDetailsDto
    {
        public int MerchantId { get; set; }

        public short RoutingId { get; set; }

        public string Logic { get; set; }

        public DateTime RowInsertionDateTime { get; set; }

        public DateTime RowUpdationDateTime { get; set; }

    }
}
