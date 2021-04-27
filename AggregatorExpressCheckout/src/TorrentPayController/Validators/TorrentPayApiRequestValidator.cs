using Core.Constants;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using ExpressCheckoutContracts.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorrentPayController.Validators
{
    public class TorrentPayApiRequestValidator: AbstractValidator<AndroidPGSdkSessionDetailDTO>
    {
        private StringBuilder m_strLogMessage;
        public TorrentPayApiRequestValidator()
        {
            m_strLogMessage = new StringBuilder();

            this.RuleSet(ConstantRuleSetName.ANDROID_SESSION_DETAIL_INSERTION_VALIDATION, () =>
            {
                RuleFor(x => x.m_iMerchantid).GreaterThan(default(int)).WithErrorCode(ResponseCodeConstants.MERCHANT_DATA_IS_INVALID.ToString());
                RuleFor(x => x.m_strTransactionId).NotNull().WithErrorCode(ResponseCodeConstants.TORRENT_PAY_MERCHANT_TRANSACTION_ID_VALIDATION_ERROR.ToString());
            });

        }
    }
}
