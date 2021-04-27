using System.ComponentModel.DataAnnotations;
using Core.Cache.Abstract;
using Core.Cache.Concrete;
using Core.Constants;
using ExpressCheckoutContracts.Constants;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Utils;
using FluentValidation;

namespace ExpressCheckoutContracts.Validators
{
    public class CustomerValidator : AbstractValidator<CustomerDto>
    {  
       


        public CustomerValidator()
        {
            
            this.RuleSet(ConstantRuleSetName.CREATE_CUSTOMER_VALIDATION, () =>
            {
                this.RuleFor(x => x.CustomerReferenceNumber.Trim()).NotEmpty().WithErrorCode(ResponseCodeConstants.INVALID_CUSTOMER_REFERENCE_NO.ToString());
                this.RuleFor(x => x.MerchantId).GreaterThan(0).WithErrorCode(ResponseCodeConstants.INVALID_MERCHANT_ID.ToString());
                this.RuleFor(x => x.MobileNumber.Trim()).NotEmpty().Must(this.ValidateMobileNumber).WithErrorCode(ResponseCodeConstants.INVALID_MOBILE_NUMBER.ToString());
                this.RuleFor(x => x.EmailId.Trim()).Must(this.ValidateMailId).When(x => !string.IsNullOrEmpty(x.EmailId)).WithErrorCode(ResponseCodeConstants.INVALID_MAIL_ID.ToString());
                this.RuleFor(x => x.CountryCode.Trim()).NotEmpty().Must(this.ValidateCountryCode).WithErrorCode(ResponseCodeConstants.INVALID_COUNTRY_CODE.ToString());
                this.RuleFor(x => x.FirstName.Trim()).NotEmpty().Must(this.ValidateName).WithErrorCode(ResponseCodeConstants.INVALID_CUSTOMER_FIRST_NAME.ToString());
                this.RuleFor(x => x.LastName.Trim()).NotEmpty().Must(this.ValidateName).WithErrorCode(ResponseCodeConstants.INVALID_CUSTOMER_LAST_NAME.ToString());
            });



            this.RuleSet(ConstantRuleSetName.UPDATE_CUSTOMER_VALIDATION, () =>
            {
              
                this.RuleFor(x => x.MerchantId).GreaterThan(0).WithErrorCode(ResponseCodeConstants.INVALID_MERCHANT_ID.ToString());
                this.RuleFor(x => x.MobileNumber.Trim()).Must(this.ValidateMobileNumber).When(x => (x.MobileNumber!=null)).WithErrorCode(ResponseCodeConstants.INVALID_MOBILE_NUMBER.ToString());
                this.RuleFor(x => x.EmailId.Trim()).Must(this.ValidateMailId).When(x => x.EmailId!=null).WithErrorCode(ResponseCodeConstants.INVALID_MAIL_ID.ToString());
                this.RuleFor(x => x.CountryCode.Trim()).NotEmpty().Must(this.ValidateCountryCode).When(x =>x.CountryCode!=null).WithErrorCode(ResponseCodeConstants.INVALID_COUNTRY_CODE.ToString());
                this.RuleFor(x => x.FirstName.Trim()).NotEmpty().Must(this.ValidateName).When(x => x.FirstName!=null).WithErrorCode(ResponseCodeConstants.INVALID_CUSTOMER_FIRST_NAME.ToString());
                this.RuleFor(x => x.LastName.Trim()).NotEmpty().Must(this.ValidateName).When(x => x.LastName!=null).WithErrorCode(ResponseCodeConstants.INVALID_CUSTOMER_LAST_NAME.ToString());
            });
        }

        private bool ValidateMailId(string emailId)
        {
            return CommonUtil.RegexMatcher(emailId, CommonConstants.EMAIL_ADDRESS_REGEX);
        }

        private bool ValidateCountryCode(string countryCode)
        {
            return CommonConstants.VALID_COUNTRY_CODES.Contains(countryCode);
        }

        private bool ValidateName(string name)
        {
            return CommonUtil.IsValidName(name);
        }



        private bool ValidateMobileNumber(string mobileNumber)
        {
            return CommonUtil.RegexMatcher(mobileNumber, CommonConstants.MOBILE_NUMBER_REGEX);
        }
    }
}