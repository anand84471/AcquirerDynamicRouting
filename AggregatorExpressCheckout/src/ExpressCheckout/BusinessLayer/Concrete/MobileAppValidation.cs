using AggExpressCheckoutDBService;
using AutoMapper;
using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using Core.Validation;
using ExpressCheckout.BusinessLayer.Abstract;
using ExpressCheckout.Validators;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Concrete;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using Core.Validation;
using ExpressCheckout.BusinessLayer.Abstract;
using ExpressCheckout.Validators;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using System.Threading.Tasks;

namespace ExpressCheckout.BusinessLayer.Concrete
{
    public class MobileAppValidation : IMobileAppValidation
    {
        private readonly IMapper _mapper;
        private IAndroidSDKService _androidSDKService;


        public MobileAppValidation(IMapper mapper, IAndroidSDKService androidSDKService
            )
        {
            _mapper = mapper;
            _androidSDKService = androidSDKService;
        }
        public async Task<BankURLDetailResponse[]> GetOtpUrlDetails(AndroidSdkGetUrlDetailsRequest objAndroidSdkGetUrlDetailsRequest)
        {
            if (objAndroidSdkGetUrlDetailsRequest == null)
            {
                throw new InvalidRequestException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }
            //var savedCardDto = _mapper.Map<SavedCardDto>(savedCardRequest);
            //ValidationHander<CustomerSavedCardsDetailsRequestValidator, SavedCardDto>.DoValidate(savedCardDto, ConstantRuleSetName.CUSTOMER_SAVED_CARD_FETCH_VALIDATION);
            //Task<bool> isSavedCardEnabledOnMerchant = IsSavedCardEnabledOnMerchant(savedCardDto);
            //Task<bool> isCustomerIdMappedWithMerchant = IsCustomerIdMappedWithMerchant(savedCardDto);


            BankOTPUrlDTO[] bankOTPUrlDTOs = await _androidSDKService.GetBankUrlDetails(objAndroidSdkGetUrlDetailsRequest);
            var arrBankDetailsResponse = _mapper.Map<BankURLDetailResponse[]>(bankOTPUrlDTOs);
            return arrBankDetailsResponse;
        }

        public async Task<bool> InsertJsExecutionError(MobileReportJsRequest mobileReportJsRequest)
        {

            return await _androidSDKService.InsertJsExecutionError(mobileReportJsRequest);
        }


        public async Task<bool> InsertAndroidSdkBrowserSessionDetails(AdnroidPGSdkSessionDetailsRequest objAdnroidPGSdkSessionDetailsRequest)
        {

            if (objAdnroidPGSdkSessionDetailsRequest == null)
            {
                throw new InvalidRequestException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }
            var androidSessionDetailDTO = _mapper.Map<AndroidPGSdkSessionDetailDTO>(objAdnroidPGSdkSessionDetailsRequest);
            ValidationHander<AndroidPGSdkRequestValidator, AndroidPGSdkSessionDetailDTO>.DoValidate(androidSessionDetailDTO, ConstantRuleSetName.ANDROID_SESSION_DETAIL_INSERTION_VALIDATION);
            return await _androidSDKService.InsertAndroidSdkBrowserSessionDetails(androidSessionDetailDTO);
        }
        public async Task<bool> ReportTransactionStatus(ReportTransactionStatusRequest objReportTransactionStatusRequest)
        {

            if (objReportTransactionStatusRequest == null)
            {
                throw new InvalidRequestException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }
            var reportTransactionStatusDto = _mapper.Map<ReportTransactionStatusDto>(objReportTransactionStatusRequest);
            return await _androidSDKService.ReportTransactionStatus(reportTransactionStatusDto);
        }
        
    }

}
