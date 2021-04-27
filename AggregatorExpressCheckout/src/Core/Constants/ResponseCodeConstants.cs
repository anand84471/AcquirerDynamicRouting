namespace Core.Constants
{
    public class ResponseCodeConstants
    {
        public const int INTERNAL_SERVER_ERROR = -2;
        public const int REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT = -1;
        public const int SUCCESS = 1;
        public const int FAILURE = -1;
        public const int MERCHANT_DATA_IS_INVALID = -272;
        public const int CUSTOMER_DATA_IS_INVALID = -338;
        public const int CARD_DATA_IS_INVALID = -307;
        public const int SAVED_CARD_IS_NOT_ENABLED_ON_MERCAHNT = -339;
        public const int INVALID_SAVED_CARD_ID = -341;
        public const int INVALID_CUSTOMER_ID_SAVED_ID_MAPPING = -343;
        public const int INVALID_CUSTOMER_ID =-342;
        public const int NO_CARD_DATA_IS_MAPPED_WITH_CUSTOMER_ID = -344;
        public const int MERCHANT_DATA_IS_NOT_PRESENT_WITH_MERCHANT = 10;
        public const int CARD_DELETION_IS_NOT_SUCCESFUL = -345;
        public const int CARD_UPDATION_IS_NOT_SUCCESFUL = -346;
        public const int SAVED_CARD_INSERTION_IS_NOT_SUCCESFUL = -347;
        public const int SAVED_CARD_FUNCTIONALITY_IS_NOT_ENABLED_ON_MERCHANT = -348;
        public const int INVALID_CUSTOMER_ID_MERCHANT_ID_MAPPING = -349;
        public const int INVALID_CUSTOMER_REFERENCE_NO = -350;
        public const int INVALID_MERCHANT_ID = -9;
        public const int INVALID_MOBILE_NUMBER = -292;
        public const int INVALID_MAIL_ID = -291;
        public const int UNABLE_TO_GENERATE_ORDER = 19;
        public const int PAYMENT_INFO_IS_NOT_VALID = 0;
        public const int AMOUNT_IS_INAVLID = 21;
        public const int CURRENCY_CODE_IS_NOT_VALID = 22;
        public const int PREFERRED_GATEWAY_IS_INVALID = 23;
        public const int INVALID_MERCHANT_ORDER_ID = 24;
        public const int HEADER_DATA_IS_MISSING = 25;
        public const int XVERIFY_HEADER_IS_NOT_PRESENT = 26;
        public const int ORDER_DATA_IS_NOT_VALID = 27;
        public const int INAVLID_SECURE_SHA = 28;
        public const int MERCHANT_IS_NOT_VALID = -271;
        public const int MERCHANT_ORDER_ID_ALREADY_EXISTS = 30;
        public const int INVALID_MERCHANT_RETURN_URL = 31;
        public const int INVALID_PAYMENT_MODE = 32;
        public const int DB_FAILURE = 1;
        public const int GATEWAY_DETAILS_NOT_PRESENT = 37;
        

        public const int MERCHANT_CONFIG_NOT_PRESENT = 33;
        public const int ORDER_CREATION_FAILED_AT_AGGREGATOR = 34;
        public const int PAYMENT_FAILED_AT_AGGREGATOR = 35;
      public const int UPDATION_FAILD_STATUS_NOT_CREATED_STATE = 35;      
       
        public const int DYNAMIC_ROUTING_DETAILS_NOT_PRESENT = 36;
     
        public const int ANDROID_BROWSER_DETAIL_INSERTION_FAIL = 38;
        public const int NOT_ENABLE_PAYMENT_MODE_FOR_MERCHANT = 39;

        public const int INVALID_COUNTRY_CODE = -351;
        public const int MERCHANT_CUSTOMER_REFERENCE_NO_ALREADY_EXIST = -352;


        public const int EITHER_MERCHANT_ROUTING_CONFIGURATION_NOT_FOUND_OR_DB_ERROR = -353;

        public const int JS_EXCEUTION_FAILS  =-354;
        public const int INVALID_CUSTOMER_FIRST_NAME= -355;
        public const int INVALID_CUSTOMER_LAST_NAME = -356;
        public const int INVALID_CARD_HOLDER_NAME = -357;
        public const int TORRENT_PAY_MERCHANT_TRANSACTION_ID_VALIDATION_ERROR = -360;
    }
}