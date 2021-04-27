using Core.Constants;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using ExpressCheckoutContracts.DTO;
using FluentValidation;

namespace ExpressCheckout.Validators
{
    public class MerchantValidator : AbstractValidator<MerchantDto>
    {
        public MerchantValidator()
        {
            this.RuleSet(ConstantRuleSetName.MERCHANT_DATA_BASIC_VALIDATION, () =>
            {
                RuleFor(x => x.MerchantId).GreaterThan(default(int)).WithErrorCode(ResponseCodeConstants.MERCHANT_DATA_IS_INVALID.ToString());
                //  RuleFor(x => x.MerchantAccessCode).NotEmpty()```````````````````````````````````````````````````````````.WithErrorCode(Constants.API_INVALID_MERCHANT_ID_ACCESS_CODE.ToString());
            });

            this.RuleSet(ConstantRuleSetName.PURCHASE_TXN_VALIDATION, () =>
            {
                RuleFor(x => x.MerchantId).GreaterThan(default(int)).WithErrorCode(ResponseCodeConstants.INVALID_MERCHANT_ID.ToString());
                RuleFor(x => x.MerchantOrderID).NotEmpty().WithErrorCode(ResponseCodeConstants.INVALID_MERCHANT_ORDER_ID.ToString());
                //RuleFor(x => x.MerchantReturnUrl).NotEmpty().WithErrorCode(ResponseCodeConstants.INVALID_MERCHANT_ORDER_ID.ToString());
                //  RuleFor(x => x.MerchantAccessCode).NotEmpty()```````````````````````````````````````````````````````````.WithErrorCode(Constants.API_INVALID_MERCHANT_ID_ACCESS_CODE.ToString());
            });
        }
    }
}