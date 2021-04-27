using Core.Constants;
using ExpressCheckoutContracts.Constants;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Utils;
using FluentValidation;
using System;
using System.Text;

namespace ExpressCheckout.Validators
{
    public class CardDataValidator : AbstractValidator<CardDetailsDto>
    {
        private StringBuilder m_strLogMessage;

        public CardDataValidator()
        {
            this.RuleSet(ConstantRuleSetName.CARD_DATA_PRESENT_IN_REQUEST_VALIDATION, () =>
            {
                RuleFor(x => x.CardNumber).Must(ValidateCardUsingLuhnAlgo).WithErrorCode(ResponseCodeConstants.CARD_DATA_IS_INVALID.ToString());
                RuleFor(x => x).Must(OnlyCheckingCardDetails).WithErrorCode(ResponseCodeConstants.CARD_DATA_IS_INVALID.ToString());
                //RuleFor(x => x.CardHolderName).NotEmpty().Must(this.ValidateName).WithErrorCode(ResponseCodeConstants.INVALID_CARD_HOLDER_NAME.ToString());
            });
            
            this.RuleSet(ConstantRuleSetName.SAVED_CARD_ID_PRESENT_VALIDATION, () =>
            {
                RuleFor(x => x.SavedCardId).GreaterThan(default(int)).WithErrorCode(ResponseCodeConstants.INVALID_SAVED_CARD_ID.ToString());
            });
        }

        private bool ValidateName(string name)
        {
            return CommonUtil.IsValidName(name);
        }


        /// <returns>true if validated successfully</returns>
        private bool ValidateCardUsingLuhnAlgo(string cardNumber)
        {
            return CreditCardValidator.ValidateCardNumber(cardNumber);
        }

        private bool OnlyCheckingCardDetails(CardDetailsDto cardData)
        {
            m_strLogMessage = new StringBuilder();
            bool result = false;
            bool isValidCVV = true, isValidMonth = true, isValidYear = true, isValidExpiryTime = true, isValidCardNumber = true, isValidCardHolderName=true;
            bool isValidMobileno = true;
            try
            {
                int year = 0;
                int cvv = 0;
                int month = 0;
                if (!cardData.IssuerId.Equals(233))
                {

                    if (String.IsNullOrEmpty(cardData.CVV))
                    {
                       
                        m_strLogMessage.Append("[CardDataValidator]OnlyCheckingCardDetails card cvv is empty");
                    }
                    else
                    {
                        isValidCVV = int.TryParse(cardData.CVV, out cvv) && cvv > 0;
                    }
                    if (String.IsNullOrEmpty(cardData.CardHolderName))
                    {
                        isValidCardHolderName = false;
                        m_strLogMessage.Append("[CardDataValidator]OnlyCheckingCardDetails CardHolderName is empty");
                    }
                    else
                    {
                        m_strLogMessage.AppendFormat("[CardDataValidator]OnlyCheckingCardDetails card holder name is:{0}", cardData.CardHolderName);
                    }
                    if (String.IsNullOrEmpty(cardData.CardExpiryMonth))
                    {
                        isValidMonth = false;
                        m_strLogMessage.Append("[CardDataValidator]OnlyCheckingCardDetails CardExpiryMonth is empty");
                    }
                    else
                    {
                        isValidMonth = int.TryParse(cardData.CardExpiryMonth, out month) && month > 0 && month < 13;
                        m_strLogMessage.AppendFormat("[CardDataValidator]OnlyCheckingCardDetails CardExpiryMonth is:{0}", cardData.CardExpiryMonth);
                    }

                    if (String.IsNullOrEmpty(cardData.CardExpiryYear))
                    {
                        m_strLogMessage.Append("[CardDataValidator]OnlyCheckingCardDetails CardExpiryYear is empty");
                    }
                    else
                    {
                        isValidYear = int.TryParse(cardData.CardExpiryYear, out year) && year > 0;
                        m_strLogMessage.AppendFormat("[CardDataValidator]OnlyCheckingCardDetails CardExpiryYear is:{0}", cardData.CardExpiryYear);
                    }

                    if (!String.IsNullOrEmpty(cardData.CardExpiryMonth))
                    {
                        isValidExpiryTime = year > DateTime.Now.Year ? true : (year == DateTime.Now.Year) ? month >= DateTime.Now.Month : false;
                    }


                }
                else
                {
                    if (String.IsNullOrEmpty(cardData.CardHolderMobileNo))
                    {
                        isValidMobileno = false;
                    }
                }
                if (String.IsNullOrEmpty(cardData.CardNumber))
                {
                    m_strLogMessage.Append("[CardDataValidator]OnlyCheckingCardDetails CardNumber is empty");
                    isValidCardNumber = false;
                }
                else
                {
                    if (cardData.CardNumber.Length < 12 || cardData.CardNumber.Length > 19)
                    {
                        isValidCardNumber = false;
                    }
                    // m_strLogMessage.AppendFormat("[CardDataValidator]OnlyCheckingCardDetailsCard bin is:{0} and length is:{1}", Utils.MaskedCardBin(cardData.CardNumber), cardData.CardNumber.Length);
                }


                result = isValidCardNumber && isValidCVV && isValidMonth && isValidYear && isValidExpiryTime && isValidCardHolderName && isValidMobileno;
                // LogMessage.LogInfoMessage(m_strLogMessage);
            }
            catch (Exception ex)
            {
                m_strLogMessage.Append("\n ----------------------------Exception Stack Trace--------------------------------------");
                m_strLogMessage.Append("Exception occured in method :" + ex.TargetSite);
                m_strLogMessage.Append(ex.ToString());
                // LogMessage.LogErrorMessage(m_strLogMessage);
            }
            return result;
        }
    }
}