using Core.Constants;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using ExpressCheckoutContracts.DTO;
using FluentValidation;

namespace ExpressCheckout.Validators
{
    public class HttpRequestInfoValidator : AbstractValidator<HttpRequestDataInfoDto>
    {
        public HttpRequestInfoValidator()
        {
            this.RuleSet(ConstantRuleSetName.PURCHASE_TXN_VALIDATION, () =>
            {
                RuleFor(x => x.XVerify).NotEmpty().WithErrorCode(ResponseCodeConstants.XVERIFY_HEADER_IS_NOT_PRESENT.ToString());
            });
        }
    }
}