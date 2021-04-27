using Core.Constants;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using ExpressCheckoutContracts.DTO;
using FluentValidation;

namespace ExpressCheckout.Validators
{
    public class PaymentInfoValidator : AbstractValidator<OrderTxnInfoDto>
    {
        public PaymentInfoValidator()
        {
            this.RuleSet(ConstantRuleSetName.PURCHASE_TXN_VALIDATION, () =>
            {
                RuleFor(x => x.Amount).GreaterThan(default(long)).WithErrorCode(ResponseCodeConstants.AMOUNT_IS_INAVLID.ToString());
                RuleFor(x => x.CurrencyCode).IsInEnum().WithErrorCode(ResponseCodeConstants.CURRENCY_CODE_IS_NOT_VALID.ToString());
                //RuleFor(x => x.PrefferedGatewayCode).Must(x => x.HasValue).WithErrorCode(ResponseCodeConstants.PREFERRED_GATEWAY_IS_INVALID.ToString());
            });
        }
    }
}