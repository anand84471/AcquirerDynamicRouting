using AggExpressCheckoutDBService;
using AutoMapper;
using ExpressCheckoutContracts;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.DTO.Routing;
using ExpressCheckoutContracts.Requests;

namespace ExpressCheckoutDb.AutoMappers
{
    internal class DtoToDBDtoMapper : Profile
    {
        public DtoToDBDtoMapper()
        {
            this.CreateMap<CardDetailsEntity[], CardDetailsDto[]>()
           .ReverseMap();

            this.CreateMap<CardDetailsEntity, CardDetailsDto>()
         .ReverseMap();

            this.CreateMap<CustomerEntity, CustomerDto>()
     .ReverseMap();
            this.CreateMap<DynamicRotuingDetailsEntity, DynamicRoutingDetailsDto>()
         .ReverseMap();

            this.CreateMap<MerchantEntity, MerchantDto>()
   .ReverseMap();
            this.CreateMap<GlobalBindataEntity, GlobalBinDataDto>()
 .ReverseMap();
            this.CreateMap<EnableGatewayListEntity, EnableGatewayPaymentModeDto>()
.ReverseMap();
            this.CreateMap<ReportTransactionStatusEntity, ReportTransactionStatusDto>().ReverseMap();


            this.CreateMap<SavedCardDto, SavedCardEntity>()
             .ForMember(dest => dest.merchantEntity, src => src.MapFrom(x => x.merchantDto))
             .ForMember(dest => dest.customerEntity, src => src.MapFrom(x => x.customerDto))
             .ForMember(dest => dest.cardDetailsEntity, src => src.MapFrom(x => x.cardDetailsDto))
             .ReverseMap();

            this.CreateMap<BankOTPUrlEntity[], BankOTPUrlDTO[]>()
          .ReverseMap();
            this.CreateMap<BankOTPUrlEntity, BankOTPUrlDTO>()
       .ReverseMap();
            this.CreateMap<AndroidSdkSessionDetailsEntity, AndroidPGSdkSessionDetailDTO>();
            #region postgress automappers
            
            #endregion
        }
    }
}