using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ExpressCheckoutContracts.Utils
{
    public class CommonUtil
    {
        public static bool RegexMatcher(string strToMatch, string strRegex)
        {
            bool result = true;
            try
            {
                result = Regex.IsMatch(strToMatch, strRegex, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public static bool IsValidEmail(string strEmail)
        {
            try
            {
                MailAddress m = new MailAddress(strEmail);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }


        public static bool IsValidName(string name)
        {
            RegularExpressionAttribute regExAttrObj = new RegularExpressionAttribute(@"[0-9A-Za-z\&\-_\|\' \/\@\,\.]*");
            bool isValid = regExAttrObj.IsValid(name);
            return isValid;
        }
    }
}
