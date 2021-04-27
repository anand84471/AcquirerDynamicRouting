using Core.Constants;
using Core.Utilities;
using ExpressCheckoutContracts.Constants;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using ExpressCheckoutContracts.DTO;
using FluentValidation;

namespace ExpressCheckout.Validators
{
    public class OrderValidator : AbstractValidator<OrderDetailsDto>
    {
        public OrderValidator()
        {
            this.RuleSet(ConstantRuleSetName.PURCHASE_TXN_VALIDATION, () =>
            {
                RuleFor(x => x.MerchantDto).Must(x => x != null).WithErrorCode(ResponseCodeConstants.MERCHANT_DATA_IS_INVALID.ToString()).SetValidator(new MerchantValidator(), ConstantRuleSetName.PURCHASE_TXN_VALIDATION);
                RuleFor(x => x.OrderTxnInfoDto).Must(x => x != null).WithErrorCode(ResponseCodeConstants.PAYMENT_INFO_IS_NOT_VALID.ToString()).SetValidator(new PaymentInfoValidator(), ConstantRuleSetName.PURCHASE_TXN_VALIDATION);
                RuleFor(x => x.CustomerDto).SetValidator(new CustomerValidator(), ConstantRuleSetName.PURCHASE_TXN_VALIDATION).When(x => x.CustomerDto != null);
                RuleFor(x => x.HttpRequestDataInfo).Must(x => x != null).WithErrorCode(ResponseCodeConstants.HEADER_DATA_IS_MISSING.ToString()).SetValidator(new HttpRequestInfoValidator(), ConstantRuleSetName.PURCHASE_TXN_VALIDATION);
                RuleFor(x => x).Must(x => x != null).Must(this.ValidateMerchantReturnUrl).WithErrorCode(ResponseCodeConstants.INVALID_MERCHANT_RETURN_URL.ToString());
            });
        }

        private bool ValidateMerchantReturnUrl(OrderDetailsDto orderDetailsDto)
        {
            bool result = true;
            MerchantDto merchantData = orderDetailsDto.MerchantDto;

            if (merchantData != null)
            {
                result = GenericUtility.RegexMatcher(merchantData.MerchantReturnUrl, CommonConstants.MERCHANT_RETURN_URL_REGEX);
                
            }
            return result;
        }
    }
}