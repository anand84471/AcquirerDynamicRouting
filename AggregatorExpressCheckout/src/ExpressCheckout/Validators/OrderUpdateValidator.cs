using Core.Constants;
using Core.Utilities;
using ExpressCheckoutContracts.Constants;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using ExpressCheckoutContracts.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressCheckout.Validators
{
    public class OrderUpdateValidator : AbstractValidator<OrderDetailsDto>
    {
        public OrderUpdateValidator()
        {
            this.RuleSet(ConstantRuleSetName.PURCHASE_TXN_VALIDATION, () =>
            {
                RuleFor(x => x.MerchantDto).Must(x => x != null).WithErrorCode(ResponseCodeConstants.MERCHANT_DATA_IS_INVALID.ToString()).SetValidator(new MerchantValidator(), ConstantRuleSetName.PURCHASE_TXN_VALIDATION);
                RuleFor(x => x.OrderTxnInfoDto).Must(x => x != null).WithErrorCode(ResponseCodeConstants.PAYMENT_INFO_IS_NOT_VALID.ToString()).SetValidator(new PaymentInfoValidator(), ConstantRuleSetName.PURCHASE_TXN_VALIDATION);
                RuleFor(x => x.HttpRequestDataInfo).Must(x => x != null).WithErrorCode(ResponseCodeConstants.HEADER_DATA_IS_MISSING.ToString()).SetValidator(new HttpRequestInfoValidator(), ConstantRuleSetName.PURCHASE_TXN_VALIDATION);
            });
        }
      
    }
}
