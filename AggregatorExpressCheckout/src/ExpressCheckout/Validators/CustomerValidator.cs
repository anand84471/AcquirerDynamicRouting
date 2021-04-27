using Core.Constants;
using ExpressCheckoutContracts.Constants;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Utils;
using FluentValidation;

namespace ExpressCheckout.Validators
{
    public class CustomerValidator : AbstractValidator<CustomerDto>
    {
        public CustomerValidator()
        {
            this.RuleSet(ConstantRuleSetName.CUSTOMER_ID_PRESENT_VALIDATION, () =>
            {
                RuleFor(x => x.CustomerId).GreaterThan(default(int)).WithErrorCode(ResponseCodeConstants.CUSTOMER_DATA_IS_INVALID.ToString());
                //  RuleFor(x => x.MerchantAccessCode).NotEmpty()```````````````````````````````````````````````````````````.WithErrorCode(Constants.API_INVALID_MERCHANT_ID_ACCESS_CODE.ToString());
            });

            this.RuleSet(ConstantRuleSetName.PURCHASE_TXN_VALIDATION, () =>
            {
                this.RuleFor(x => x.EmailId).Must(this.ValidateMailId).When(x => !string.IsNullOrEmpty(x.EmailId)).WithErrorCode(ResponseCodeConstants.INVALID_MAIL_ID.ToString());
                RuleFor(x => x.MobileNumber).Must(this.ValidateMobileNumber).When(x => !string.IsNullOrEmpty(x.MobileNumber)).WithErrorCode(ResponseCodeConstants.INVALID_MOBILE_NUMBER.ToString());
            });
        }

        private bool ValidateMailId(string emailId)
        {
            return CommonUtil.IsValidEmail(emailId);
        }

        private bool ValidateMobileNumber(string mobileNumber)
        {
            return CommonUtil.RegexMatcher(mobileNumber, CommonConstants.MOBILE_NUMBER_REGEX);
        }
    }
}