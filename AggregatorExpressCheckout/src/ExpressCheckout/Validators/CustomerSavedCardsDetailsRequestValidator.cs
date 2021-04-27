using ExpressCheckoutContracts.DTO;
using FluentValidation;
using System.Text;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using Core.Constants;

namespace ExpressCheckout.Validators
{
    public class CustomerSavedCardsDetailsRequestValidator : AbstractValidator<SavedCardDto>
    {
        private StringBuilder m_strLogMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardDataValidator"/> class.
        /// </summary>
        public CustomerSavedCardsDetailsRequestValidator()
        {
            m_strLogMessage = new StringBuilder();

            this.RuleSet(ConstantRuleSetName.CUSTOMER_SAVED_CARD_INSERTION_VALIDATION, () =>
            {
                RuleFor(x => x.merchantDto).Must(x => x != null).WithErrorCode(ResponseCodeConstants.MERCHANT_DATA_IS_INVALID.ToString()).SetValidator(new MerchantValidator(), ConstantRuleSetName.MERCHANT_DATA_BASIC_VALIDATION);
                RuleFor(x => x.customerDto).Must(x => x != null).WithErrorCode(ResponseCodeConstants.CUSTOMER_DATA_IS_INVALID.ToString()).SetValidator(new CustomerValidator(), ConstantRuleSetName.CUSTOMER_ID_PRESENT_VALIDATION);
                RuleFor(x => x.cardDetailsDto).Must(x => x != null).WithErrorCode(ResponseCodeConstants.CARD_DATA_IS_INVALID.ToString()).SetValidator(new CardDataValidator(), ConstantRuleSetName.CARD_DATA_PRESENT_IN_REQUEST_VALIDATION);
                // RuleFor(x => x).MustAsync(this.IsSavedCardEnabledOnMerchant).WithErrorCode(ResponseCodeConstants.SAVED_CARD_IS_NOT_ENABLED_ON_MERCAHNT.ToString());
            });

            this.RuleSet(ConstantRuleSetName.CUSTOMER_SAVED_CARD_FETCH_VALIDATION, () =>
            {
                RuleFor(x => x.merchantDto).Must(x => x != null).WithErrorCode(ResponseCodeConstants.MERCHANT_DATA_IS_INVALID.ToString()).SetValidator(new MerchantValidator(), ConstantRuleSetName.MERCHANT_DATA_BASIC_VALIDATION);
                RuleFor(x => x.customerDto).Must(x => x != null).WithErrorCode(ResponseCodeConstants.CUSTOMER_DATA_IS_INVALID.ToString()).SetValidator(new CustomerValidator(), ConstantRuleSetName.CUSTOMER_ID_PRESENT_VALIDATION);
                //RuleFor(x => x).MustAsync(this.IsCustomerIdValid).WithErrorCode(ResponseCodeConstants.INVALID_CUSTOMER_ID.ToString());
            });

            this.RuleSet(ConstantRuleSetName.CUSTOMER_SAVED_CARD_DELETE_VALIDATION, () =>
            {
                RuleFor(x => x.merchantDto).Must(x => x != null).WithErrorCode(ResponseCodeConstants.MERCHANT_DATA_IS_INVALID.ToString()).SetValidator(new MerchantValidator(), ConstantRuleSetName.MERCHANT_DATA_BASIC_VALIDATION);
                RuleFor(x => x.customerDto).Must(x => x != null).WithErrorCode(ResponseCodeConstants.CUSTOMER_DATA_IS_INVALID.ToString()).SetValidator(new CustomerValidator(), ConstantRuleSetName.CUSTOMER_ID_PRESENT_VALIDATION);
                RuleFor(x => x.cardDetailsDto).Must(x => x != null).WithErrorCode(ResponseCodeConstants.INVALID_SAVED_CARD_ID.ToString()).SetValidator(new CardDataValidator(), ConstantRuleSetName.SAVED_CARD_ID_PRESENT_VALIDATION);
                // RuleFor(x => x).MustAsync(this.IsSavedCardDeletionValid).WithErrorCode(ResponseCodeConstants.INVALID_CUSTOMER_ID_SAVED_ID_MAPPING.ToString());
            });
        }

        //private async Task<bool> IsSavedCardEnabledOnMerchant(SavedCardDto savedCardDto, CancellationToken cancellationToken)
        //{
        //    MerchantDto merchantDto = savedCardDto.merchantDto;
        //    return await _MerchantService.IsSavedCardEnabled(merchantDto.MerchantId);
        //}

        //private async Task<bool> IsCustomerIdMappedWithMerchant(SavedCardDto savedCardDto)
        //{
        //    MerchantDto merchantDto = savedCardDto.merchantDto;
        //    CustomerDto customerDto = savedCardDto.customerDto;
        //    return await _CustomerService.CheckIsCustomerIdMappedWithMerchant(merchantDto.MerchantId, customerDto.CustomerId);
        //}

        //private async Task<bool> IsSavedCardIdMappedWithCustomerId(SavedCardDto savedCardDto)
        //{
        //    CardDetailsDto cardDetailsDto = savedCardDto.cardDetailsDto;
        //    CustomerDto customerDto = savedCardDto.customerDto;
        //    return await _CustomerSavedCardService.CheckIsSavedCardIdMappedWithCustomer(customerDto.CustomerId, cardDetailsDto.SavedCardId);
        //}

        //public async Task<bool> IsCustomerIdValid(SavedCardDto savedCardDto, CancellationToken cancellationToken)
        //{
        //    bool IsSavedCardEnabled = await IsSavedCardEnabledOnMerchant(savedCardDto, cancellationToken);
        //    bool IsCustomerIdMappedWIthMerchant = await IsCustomerIdMappedWithMerchant(savedCardDto);
        //    return (IsSavedCardEnabled && IsCustomerIdMappedWIthMerchant);
        //}

        //public async Task<bool> IsSavedCardDeletionValid(SavedCardDto savedCardDto, CancellationToken cancellationToken)
        //{
        //    bool isCustomerIdValid = await IsCustomerIdValid(savedCardDto, cancellationToken);
        //    bool IsSavedCardIdValid = await IsSavedCardIdMappedWithCustomerId(savedCardDto);
        //    return (isCustomerIdValid && IsSavedCardIdValid);
        //}
    }
}