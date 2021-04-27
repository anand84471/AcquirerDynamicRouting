using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Constants;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;
using FluentValidation;

namespace ExpressCheckout.Validators
{

    public class AndroidPGSdkRequestValidator : AbstractValidator<AndroidPGSdkSessionDetailDTO>
    {
        private StringBuilder m_strLogMessage;

        public AndroidPGSdkRequestValidator()
        {
            m_strLogMessage = new StringBuilder();

            this.RuleSet(ConstantRuleSetName.ANDROID_SESSION_DETAIL_INSERTION_VALIDATION, () =>
            {
                RuleFor(x => x.m_iMerchantid).GreaterThan(default(int)).WithErrorCode(ResponseCodeConstants.MERCHANT_DATA_IS_INVALID.ToString());
                RuleFor(x => x.m_strCustomeId).NotNull().WithErrorCode(ResponseCodeConstants.CUSTOMER_DATA_IS_INVALID.ToString());
            });

        }
    }
}
