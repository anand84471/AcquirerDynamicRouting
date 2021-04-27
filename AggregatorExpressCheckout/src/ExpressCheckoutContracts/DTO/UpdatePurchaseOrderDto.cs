using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressCheckoutContracts.DTO
{
  public class UpdatePurchaseOrderDto
    {
        public HttpRequestDataInfoDto HttpRequestDataInfo { get; set; }

        public MerchantDto MerchantDto { get; set; }

        public OrderTxnInfoDto OrderTxnInfoDto { get; set; }

        public BillingAddressDto BillingAddressDto { get; set; }

        public ShippingAddressDto ShippingAddressDto { get; set; }

        public string JsonRequestString { get; set; }

    }
}
