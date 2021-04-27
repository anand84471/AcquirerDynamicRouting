using AggExpressCheckoutDBService;
using AutoMapper;
using ExpressCheckoutContracts;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.Razorpay;
using ExpressCheckoutContracts.DTO.Routing;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.ExternalApis.Responses;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Concrete;
using ExpressCheckoutDb.Entities;
using ExpressCheckoutDb.Entities.Concrete;

namespace ExpressCheckoutModule.Infrastructure.Automapper
{
    public class AutomapperMappingProfile : Profile
    {
        public AutomapperMappingProfile()
        {
            this.InitializeMappingProfiles();
            this.RazorPayMappingProfile();
            this.RoutingMappingProfile();
            this.TorrentPayMappingProfile();
        }

        private void InitializeMappingProfiles()
        {
            //----------------------------------------REQUEST TO DTO-------------------------------
            this.CreateMap<CustomerRequest, CustomerDto>().ReverseMap();
            this.CreateMap<CreateCustomerRequest, CustomerDto>().ReverseMap();
            this.CreateMap<MerchantRequest, MerchantDto>().ReverseMap();
            this.CreateMap<CardRequest, CardDetailsDto>().ReverseMap();
            this.CreateMap<OrderTxnInfoRequest, OrderTxnInfoDto>().ReverseMap();
            this.CreateMap<BillingAddressRequest, BillingAddressDto>().ReverseMap();
            this.CreateMap<ShippingAddressRequest, ShippingAddressDto>().ReverseMap();
            this.CreateMap<AdditonalDataRequest, AdditonalDataDto>().ReverseMap();
            this.CreateMap<CustomHeader, HttpRequestDataInfoDto>().ReverseMap();
            this.CreateMap<ProductDetailsRequest, ProductDetailsDto>().ReverseMap();
            this.CreateMap<ProductInfoRequest, ProductInfoDto>().ReverseMap();
            this.CreateMap<BankURLDetailResponse, BankOTPUrlDTO>().ReverseMap();
            this.CreateMap<CustomerDto, CustomerResponse>();
            this.CreateMap<UpdateCustomerRequest, CustomerDto>().ReverseMap();
            this.CreateMap<CustomerEntity, CustomerDto>().ReverseMap();
            this.CreateMap<CustomerResponse, CustomerDto>().ReverseMap();
            this.CreateMap<ReportTransactionStatusRequest, ReportTransactionStatusDto>().ReverseMap();
            this.CreateMap<GlobalBinDataDto, GlobalBinsData>().ReverseMap();

            this.CreateMap<SavedCardRequest, SavedCardDto>()
                .ForMember(dest => dest.merchantDto, src => src.MapFrom(x => x.merchantRequest))
                .ForMember(dest => dest.customerDto, src => src.MapFrom(x => x.customerRequest))
                .ForMember(dest => dest.cardDetailsDto, src => src.MapFrom(x => x.cardRequest))
                .ReverseMap();
            this.CreateMap<AdnroidPGSdkSessionDetailsRequest, AndroidPGSdkSessionDetailDTO>().ReverseMap();

            this.CreateMap<OrderDetailsRequest, OrderDetailsDto>()
             .ForMember(dest => dest.MerchantDto, src => src.MapFrom(x => x.MerchantRequest))
             .ForMember(dest => dest.CustomerDto, src => src.MapFrom(x => x.CustomerRequest))
             .ForMember(dest => dest.OrderTxnInfoDto, src => src.MapFrom(x => x.OrderTxnInfoRequest))
             .ForMember(dest => dest.BillingAddressDto, src => src.MapFrom(x => x.BillingAddressRequest))
             .ForMember(dest => dest.ShippingAddressDto, src => src.MapFrom(x => x.ShippingAddressRequest))
             .ForMember(dest => dest.ProductInfoDto, src => src.MapFrom(x => x.ProductInfoRequest))
           //  .ForPath(dest => dest.productInfoDto.productDetails, src => src.MapFrom(x => x.productInfoRequest.productDetails))
             .ForMember(dest => dest.additional_info_data, src => src.MapFrom(x => x.AdditonalDataRequest));


            this.CreateMap<AcceptOrderDetailsRequest, OrderDetailsDto>()
                   .ForMember(dest => dest.HttpRequestDataInfo, src => src.MapFrom(x => x.CustomHeader));


            this.CreateMap<DependentOrderRequest, OrderDetailsDto>()
                .ForMember(dest => dest.MerchantDto, src => src.MapFrom(x => x.merchantRequest))
             .ForMember(dest => dest.OrderTxnInfoDto, src => src.MapFrom(x => x.orderTxnInfoRequest));


            //-------------------------------------DTO TO ENTITY------------------------------------

            this.CreateMap<BankOTPUrlEntity, BankOTPUrlDTO>()
            .ReverseMap();
            this.CreateMap<CardDetailsEntity, CardDetailsDto>().ReverseMap();
            this.CreateMap<EnableGatewayListEntity, EnableGatewayPaymentModeDto>().ReverseMap();
            this.CreateMap<CustomerEntity, CustomerDto>().ReverseMap();
            this.CreateMap<MerchantEntity, MerchantDto>().ReverseMap();
            this.CreateMap<OrderTxnInfoEntity, OrderTxnInfoDto>().ReverseMap();
            this.CreateMap<BillingAddressEntity, BillingAddressDto>().ReverseMap();
            this.CreateMap<ShippingAddressEntity, ShippingAddressDto>().ReverseMap();
            this.CreateMap<AdditonalDataEntity, AdditonalDataDto>().ReverseMap();
            this.CreateMap<ProductDetailsEntity, ProductDetailsDto>().ReverseMap();
            this.CreateMap<ProductInfoEntity, ProductInfoDto>().ReverseMap();
            this.CreateMap<PaymentDataEntity, PaymentDataDto>().ReverseMap();
            this.CreateMap<GatewayEntity, GatewayDto>().ReverseMap();
            this.CreateMap<MerchantGatewayConfigurationMappingEntity, MerchantGatewayConfigurationMappingDto>().ReverseMap();
            this.CreateMap<DynamicRotuingDetailsEntity, DynamicRoutingDetailsDto>().ReverseMap();
            this.CreateMap<GlobalBindataEntity, GlobalBinDataDto>().ReverseMap();
            this.CreateMap<ReportTransactionStatusEntity, ReportTransactionStatusDto>().ReverseMap();
            this.CreateMap<SavedCardDto, SavedCardEntity>()
             .ForMember(dest => dest.merchantEntity, src => src.MapFrom(x => x.merchantDto))
             .ForMember(dest => dest.customerEntity, src => src.MapFrom(x => x.customerDto))
             .ForMember(dest => dest.cardDetailsEntity, src => src.MapFrom(x => x.cardDetailsDto))
             .ReverseMap();

            this.CreateMap<CustomerResponse, CustomerDto>().ReverseMap();
            this.CreateMap<OrderDetailsDto, OrderDetailsEntity>()
             .ForMember(dest => dest.merchantEntity, src => src.MapFrom(x => x.MerchantDto))
             .ForMember(dest => dest.customerEntity, src => src.MapFrom(x => x.CustomerDto))
             .ForMember(dest => dest.OrderTxnInfoEntity, src => src.MapFrom(x => x.OrderTxnInfoDto))
             .ForMember(dest => dest.billingAddressEntity, src => src.MapFrom(x => x.BillingAddressDto))
             .ForMember(dest => dest.shippingAddressEntity, src => src.MapFrom(x => x.ShippingAddressDto))
             .ForMember(dest => dest.productInfoEntity, src => src.MapFrom(x => x.ProductInfoDto))
             .ForMember(dest => dest.additonalDataEntity, src => src.MapFrom(x => x.additional_info_data))
             .ForMember(dest=>dest.paymentDataEntity, src => src.MapFrom(x => x.PaymentDataDto))
             .ForMember(dest => dest.Bindata, src => src.MapFrom(x => x.BinData))
             .ReverseMap();

            this.CreateMap<OrderStepDto, OrderStepEntity>().ReverseMap();
            this.CreateMap<OrderPaymentDetailsDto, OrderPaymentDetailsInsert>().ReverseMap();
            this.CreateMap<OrderPaymentDetailsDto, OrderPaymentDetails>().ReverseMap();
            this.CreateMap<UpdateOrderDetailsDto, UpdateOrderDetails>().ReverseMap();


            this.CreateMap<AndroidPGSdkSessionDetailDTO, AndroidSdkSessionDetailsEntity>()            
            .ReverseMap();

        }

        private void RazorPayMappingProfile()
        {
            this.CreateMap<OrderDetailsDto, RazorpayOrderRequestDto>()
                .ForMember(dest => dest.Amount, src => src.MapFrom(x => x.OrderTxnInfoDto.Amount))
                .ForMember(dest => dest.Currency, src => src.MapFrom(x => x.OrderTxnInfoDto.CurrencyCode))
                .ForMember(dest => dest.Receipt, src => src.MapFrom(x => x.OrderTxnInfoDto.AggOrderId.ToString()));
        }

        private void RoutingMappingProfile()
        {
            this.CreateMap<MerchantRoutingConfigDetailsDto, MerchantRoutingConfigDetailsEntity>()
       .ReverseMap();

            this.CreateMap<SpecialRoutingDetailsDto, SpecialRoutingDetailsEntity>()
        .ReverseMap();
        }
        private void TorrentPayMappingProfile()
        {
            this.CreateMap<TorrentPayBankOTPUrlDetailEntity, BankOTPUrlDTO>().ReverseMap();
            this.CreateMap<MobileReportJsRequest, TorrentPayJsExceptionEntity>().ReverseMap();
            this.CreateMap<TorrentPaySessionDetailsEntity, AndroidPGSdkSessionDetailDTO>().ReverseMap();
            this.CreateMap<ReportTransactionStatusDto, TorrentPayChangeTxnStatusEntity>().ReverseMap();
            this.CreateMap<ReportTransactionStatusRequest, ReportTransactionStatusDto>().ReverseMap();
            this.CreateMap<TorrentPayTransactionDetailsRequest, AndroidPGSdkSessionDetailDTO>().ReverseMap();
        }

    }
}