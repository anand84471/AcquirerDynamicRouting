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
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExpressCheckout.BusinessLayer.Concrete
{
    public class CustomerSavedCardValidation : ICustomerSavedCardValidation
    {
        private readonly IMapper _mapper;
        private ICustomerSavedCardService _CustomerSavedCardService;
        private IMerchantService _MerchantService;
        private ICustomerService _CustomerService;
        private readonly ILogger<CustomerSavedCardValidation> _logger;
        public CustomerSavedCardValidation(IMapper mapper, ICustomerSavedCardService customerSavedCardService,
            IMerchantService MerchantService, ICustomerService CustomerService, ILogger<CustomerSavedCardValidation> _logger)
        {
            _mapper = mapper;
            _CustomerSavedCardService = customerSavedCardService;
            _MerchantService = MerchantService;
            _CustomerService = CustomerService;
            this._logger = _logger;
        }

        public async Task<CardRequest> InsertCustomerSavedCardValidation(SavedCardRequest savedCardRequest)
        {

            if (savedCardRequest == null)
            {
                this._logger.LogError("Method{0} Request is empty", "InsertCustomerSavedCardValidation");
                throw new InvalidRequestException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }
            var savedCardDto = _mapper.Map<SavedCardDto>(savedCardRequest);
            ValidationHander<CustomerSavedCardsDetailsRequestValidator, SavedCardDto>.DoValidate(savedCardDto, ConstantRuleSetName.CUSTOMER_SAVED_CARD_INSERTION_VALIDATION);
            if (!await _MerchantService.IsSavedCardEnabled(savedCardDto.merchantDto.MerchantId))
            {
                this._logger.LogError("Method{0} Saved card is not enabled on Merchant Id {1}", "InsertCustomerSavedCardValidation", savedCardDto.merchantDto.MerchantId);
                throw new InvalidRequestException(ResponseCodeConstants.SAVED_CARD_FUNCTIONALITY_IS_NOT_ENABLED_ON_MERCHANT);
            }

            Task<bool> isCustomerIdMappedWithMerchant = IsCustomerIdMappedWithMerchant(savedCardDto);
            if (!await isCustomerIdMappedWithMerchant)
            {
                this._logger.LogError("Method{0} Customer Id is not mapped with Merchant", "InsertCustomerSavedCardValidation");
                throw new InvalidRequestException(ResponseCodeConstants.INVALID_CUSTOMER_ID_MERCHANT_ID_MAPPING);
            }
            await _CustomerSavedCardService.InsertSavedCardDetails(savedCardDto);
            var CardDetailsResponse = _mapper.Map<CardRequest>(savedCardDto.cardDetailsDto);           
            return CardDetailsResponse;
        }

        public async Task<CardRequest[]> GetAllSaveCard(SavedCardRequest savedCardRequest)
        {
            if (savedCardRequest == null)
            {
                this._logger.LogError("Method{0} Request is empty", "GetAllSaveCard");
                throw new InvalidRequestException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }
            var savedCardDto = _mapper.Map<SavedCardDto>(savedCardRequest);
            ValidationHander<CustomerSavedCardsDetailsRequestValidator, SavedCardDto>.DoValidate(savedCardDto, ConstantRuleSetName.CUSTOMER_SAVED_CARD_FETCH_VALIDATION);
            Task<bool> isSavedCardEnabledOnMerchant = IsSavedCardEnabledOnMerchant(savedCardDto);
            Task<bool> isCustomerIdMappedWithMerchant = IsCustomerIdMappedWithMerchant(savedCardDto);

            if (!await isSavedCardEnabledOnMerchant)
            {
                this._logger.LogError("Method{0} Saved card is not enabled on Merchant Id {1}", "GetAllSaveCard", savedCardDto.merchantDto.MerchantId);

                 throw new InvalidRequestException(ResponseCodeConstants.SAVED_CARD_FUNCTIONALITY_IS_NOT_ENABLED_ON_MERCHANT);
            }
            if (!await isCustomerIdMappedWithMerchant)
            {

                this._logger.LogError("Method{0} Customer Id is not mapped with Merchant", "GetAllSaveCard");
                throw new InvalidRequestException(ResponseCodeConstants.INVALID_CUSTOMER_ID_MERCHANT_ID_MAPPING);
            }

            CardDetailsDto[] arrCardDetailsDto = await _CustomerSavedCardService.GetAllSavedCards(savedCardDto.customerDto.CustomerId);
            var arrCardDetailsResponse = _mapper.Map<CardRequest[]>(arrCardDetailsDto);
            return arrCardDetailsResponse;
        }

        public async Task<bool> DeleteSaveCard(SavedCardRequest savedCardRequest)
        {
            if (savedCardRequest == null)
            {
                this._logger.LogError("Method{0} Request is empty", "DeleteSaveCard");
                throw new InvalidRequestException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }
            var savedCardDto = _mapper.Map<SavedCardDto>(savedCardRequest);
            ValidationHander<CustomerSavedCardsDetailsRequestValidator, SavedCardDto>.DoValidate(savedCardDto, ConstantRuleSetName.CUSTOMER_SAVED_CARD_DELETE_VALIDATION);
            Task<bool> isSavedCardEnabledOnMerchant = IsSavedCardEnabledOnMerchant(savedCardDto);
            Task<bool> isCustomerIdMappedWithMerchant = IsCustomerIdMappedWithMerchant(savedCardDto);
            Task<bool> isSavedCardIdValid = IsSavedCardIdMappedWithCustomerId(savedCardDto);
            if (!await isSavedCardEnabledOnMerchant)
            {
                this._logger.LogError("Method{0} Saved card is not enabled on Merchant Id {1}", "DeleteSaveCard", savedCardDto.merchantDto.MerchantId);

                throw new InvalidRequestException(ResponseCodeConstants.SAVED_CARD_FUNCTIONALITY_IS_NOT_ENABLED_ON_MERCHANT);
            }
            if (!await isCustomerIdMappedWithMerchant)
            {
                this._logger.LogError("Method{0} Customer Id {2} not mapped with Merchant {1}", "DeleteSaveCard", savedCardDto.merchantDto.MerchantId,savedCardDto.customerDto.CustomerId);
                throw new InvalidRequestException(ResponseCodeConstants.INVALID_CUSTOMER_ID_MERCHANT_ID_MAPPING);
            }
            if (!await isSavedCardIdValid)
            {
                this._logger.LogError("Method{0} Saved card Id is not valid  {1}", "DeleteSaveCard", savedCardDto.cardDetailsDto.SavedCardId);
                throw new InvalidRequestException(ResponseCodeConstants.INVALID_CUSTOMER_ID_SAVED_ID_MAPPING);
            }
            return await _CustomerSavedCardService.DeleteSavedCardDetails(savedCardDto.customerDto.CustomerId, savedCardDto.cardDetailsDto.SavedCardId);
        }


        public async Task<bool> UpdateSaveCardStatus(SavedCardRequest savedCardRequest)
        {
            if (savedCardRequest == null)
            {
                this._logger.LogError("Method{0} Request is empty", "UpdateSaveCardStatus");
                throw new InvalidRequestException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }
            var savedCardDto = _mapper.Map<SavedCardDto>(savedCardRequest);
            ValidationHander<CustomerSavedCardsDetailsRequestValidator, SavedCardDto>.DoValidate(savedCardDto, ConstantRuleSetName.CUSTOMER_SAVED_CARD_DELETE_VALIDATION);
            Task<bool> isSavedCardEnabledOnMerchant = IsSavedCardEnabledOnMerchant(savedCardDto);
            Task<bool> isCustomerIdMappedWithMerchant = IsCustomerIdMappedWithMerchant(savedCardDto);
           // Task<bool> isSavedCardIdValid = IsSavedCardIdMappedWithCustomerId(savedCardDto);
            if (!await isSavedCardEnabledOnMerchant)
            {
                this._logger.LogError("Method{0} Saved card is not enabled on Merchant Id {1}", "UpdateSaveCardStatus", savedCardDto.merchantDto.MerchantId);
                throw new InvalidRequestException(ResponseCodeConstants.SAVED_CARD_FUNCTIONALITY_IS_NOT_ENABLED_ON_MERCHANT);
            }
            if (!await isCustomerIdMappedWithMerchant)
            {
                this._logger.LogError("Method{0} Customer Id {2} not mapped with Merchant {1}", "UpdateSaveCardStatus", savedCardDto.merchantDto.MerchantId, savedCardDto.customerDto.CustomerId);
                throw new InvalidRequestException(ResponseCodeConstants.INVALID_CUSTOMER_ID_MERCHANT_ID_MAPPING);
            }
           
            return await _CustomerSavedCardService.UpdateSavedCardStatus(savedCardDto.customerDto.CustomerId, savedCardDto.cardDetailsDto.SavedCardId, savedCardDto.cardDetailsDto.status);
        }

        private async Task<bool> IsSavedCardEnabledOnMerchant(SavedCardDto savedCardDto)
        {
            MerchantDto merchantDto = savedCardDto.merchantDto;
            return await _MerchantService.IsSavedCardEnabled(merchantDto.MerchantId);
        }

        private async Task<bool> IsCustomerIdMappedWithMerchant(SavedCardDto savedCardDto)
        {
            MerchantDto merchantDto = savedCardDto.merchantDto;
            CustomerDto customerDto = savedCardDto.customerDto;
            return await _CustomerService.CheckIsCustomerIdMappedWithMerchant(merchantDto.MerchantId, customerDto.CustomerId);
        }

        private async Task<bool> IsSavedCardIdMappedWithCustomerId(SavedCardDto savedCardDto)
        {
            CardDetailsDto cardDetailsDto = savedCardDto.cardDetailsDto;
            CustomerDto customerDto = savedCardDto.customerDto;
            return await _CustomerSavedCardService.CheckIsSavedCardIdMappedWithCustomer(customerDto.CustomerId, cardDetailsDto.SavedCardId);
        }

        //public async Task<bool> IsCustomerIdValid(SavedCardDto savedCardDto)
        //{
        //    bool IsSavedCardEnabled = await IsSavedCardEnabledOnMerchant(savedCardDto);
        //    bool IsCustomerIdMappedWIthMerchant = await IsCustomerIdMappedWithMerchant(savedCardDto);
        //    return (IsSavedCardEnabled && IsCustomerIdMappedWIthMerchant);
        //}

        //public async Task<bool> IsSavedCardDeletionValid(SavedCardDto savedCardDto)
        //{
        //    bool isCustomerIdValid = await IsCustomerIdValid(savedCardDto);
        //    bool IsSavedCardIdValid = await IsSavedCardIdMappedWithCustomerId(savedCardDto);
        //    return (isCustomerIdValid && IsSavedCardIdValid);
        //}



    }
}