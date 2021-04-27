using System.Collections.Generic;

namespace ExpressCheckoutContracts.DTO
{
    public class OrderDetailsDto
    {
        public HttpRequestDataInfoDto HttpRequestDataInfo { get; set; }

        public MerchantDto MerchantDto { get; set; }

        public OrderTxnInfoDto OrderTxnInfoDto { get; set; }

        public CustomerDto CustomerDto { get; set; }

        public BillingAddressDto BillingAddressDto { get; set; }

        public ShippingAddressDto ShippingAddressDto { get; set; }

        public ProductInfoDto ProductInfoDto { get; set; }

        public AdditonalDataDto additional_info_data { get; set; }

        public string JsonRequestString { get; set; }

        public PaymentDataDto PaymentDataDto { get; set; }

        public MerchantGatewayConfigurationMappingDto MerchantGatewayConfigurationMappingDto { get; set; }

        public GatewayDto GatewayDto { get; set; }

        public GlobalBinDataDto BinData { get; set; }
    }
}