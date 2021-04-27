using System.Collections.Generic;

namespace ExpressCheckoutContracts.Constants
{
    public class CommonConstants
    {
        public const string MOBILE_NUMBER_REGEX = @"^([0-9]){10}$";
        public const string MERCHANT_RETURN_URL_REGEX = @"^http(s)?://([\w-]+.)+[\w-]+(\/?[\w- ./?%&=])?$";
        public static  List<string> VALID_COUNTRY_CODES = new List<string> { "91" };
        public const string EMAIL_ADDRESS_REGEX = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$";
        public const string NAME_REGEX = @"[0-9A-Za-z\&\-_\|\' \/\@\,\.]*";


        #region Razorpay

        public const string RAZORPAY_CARD_METHOD = "card";
        public const string RAZORPAY_NETBANKING_METHOD = "netbanking";

        #endregion


        public const string CARD_TYPE_CREDIT = "CREDIT";
        public const string CARD_TYPE_DEBIT = "DEBIT";
        public const string CARD_TYPE_OTHERS = "OTHERS";
    }
}