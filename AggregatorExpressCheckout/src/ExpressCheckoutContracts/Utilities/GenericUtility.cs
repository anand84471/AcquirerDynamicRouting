using Core.Constants;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Response.Concrete;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Core.Utilities
{
    public class GenericUtility
    {
        public static string toQueryString(NameValueCollection nvRequest)
        {
            var array = (from key in nvRequest.AllKeys
                         from value in nvRequest.GetValues(key)
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
                .ToArray();
            return string.Join("&", array);
        }

        public static Uri CreateUri(string uri)
        {
            if (!string.IsNullOrEmpty(uri))
            {
                return new Uri(uri, UriKind.RelativeOrAbsolute);
            }
            return null;
        }

        /// <summary>
        /// Method used to Mask the User Card Number(LEFT_PADDING_CARDLENGTH = 4,  RIGHT_PADDING_CARDLENGTH = 6)
        /// </summary>
        /// <param name="strCardNumber">
        /// Input Card Number
        /// </param>
        /// <returns>
        /// Masked Card number
        /// </returns>
        public static string MaskCardNumber(string strCardNumber)
        {
            try
            {
                int iLeftPaddingLength = CardConstants.LEFT_PADDING_CARDLENGTH;
                int iRightPaddingLength = CardConstants.RIGHT_PADDING_CARDLENGTH;
                //var strCardNumber = "1234567890";

                if (strCardNumber.Length < (iLeftPaddingLength + iRightPaddingLength))
                {
                    return null;
                }
                else if (strCardNumber.Length == (iLeftPaddingLength + iRightPaddingLength))
                {
                    return strCardNumber; //Dont replace
                }

                string strMaskedNumber = strCardNumber;
                int lenToBeMasked = 0;
                //Get Left 6
                strMaskedNumber = strMaskedNumber.Substring(0, iLeftPaddingLength);// .Left(6);

                lenToBeMasked = strCardNumber.Length - (iLeftPaddingLength + iRightPaddingLength);

                //Add Mask
                for (int i = 0; i < lenToBeMasked; i++)
                {
                    strMaskedNumber = strMaskedNumber + "*";
                }
                //Get Last 4
                strMaskedNumber = strMaskedNumber + strCardNumber.Substring(strCardNumber.Length - iRightPaddingLength, iRightPaddingLength);

                return strMaskedNumber;

                //int maskedLength = strCardNumber.Length - (iLeftPaddingLength + iRightPaddingLength);
                //string maskingString = new string('*', maskedLength);
                //string strMaskedString = strCardNumber.Replace(strCardNumber.Substring(iLeftPaddingLength, maskedLength), maskingString);

                //return strMaskedString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetSHAGenerated(string request, string secureSecret)
        {
            string hexHash = String.Empty;

            byte[] convertedHash = new byte[secureSecret.Length / 2];
            for (int i = 0; i < secureSecret.Length / 2; i++)
            {
                convertedHash[i] = (byte)int.Parse(secureSecret.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }


            using (HMACSHA256 hasher = new HMACSHA256(convertedHash))
            {
                byte[] hashValue = hasher.ComputeHash(Encoding.UTF8.GetBytes(request));
                foreach (byte b in hashValue)
                {
                    hexHash += b.ToString("X2");
                }
            }


            return hexHash;
        }

        public static string GetHash512(string request) 
        {
            byte[] message = Encoding.UTF8.GetBytes(request);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        public static bool ValidateSecureIncomingRequest(string request, string xVerify, string secureSeret)
        {
            return xVerify.Equals(GetSHAGenerated(request, secureSeret));
        }

        public static T GetObjectFromJsonString<T>(string jsonString)
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static T GetObjectFromBase64EncodedJsonRequest<T>(string base64EncodeRequest)
            where T : class
        {
            T obj = null;
            var requestData = DecodeBase64String(base64EncodeRequest);
            if (!string.IsNullOrEmpty(requestData))
            {
                obj = JsonConvert.DeserializeObject<T>(requestData);
            }

            return obj;


        }

        public static string GetBase64FromObject<T>(T obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            return base64String;
        }

        public static string GetJsonStringFromBase64EncodeStriing(string base64EncodeRequest)
        {
            return DecodeBase64String(base64EncodeRequest);
        }

        public static T ConvertJsonStringToObject<T>(string jsonString)
             where T : class
        {
            T obj = null;
            if (!string.IsNullOrEmpty(jsonString))
            {
                obj = JsonConvert.DeserializeObject<T>(jsonString);
            }

            return obj;
        }

        public static string ConvertObjectToJsonString<T>(T obj) where T : class
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static bool ValidateForXSSAttackAttempt<T>(T obj)
            where T : class
        {
            var json = JsonConvert.SerializeObject(obj);
            return json.Contains("<script>") || json.Contains("<SCRIPT>");
        }

        public static string GetJsonStringFromHttpRequestHeader(IHeaderDictionary headers)
        {
            string jsonForObj = String.Empty;
            try
            {
                jsonForObj = "{";
                foreach (var header in headers)
                {
                    jsonForObj += "\"" + header.Key + "\"" + ":" + "\"" + headers[header.Key].First() + "\"" + ",";
                }
                jsonForObj = jsonForObj.Remove(jsonForObj.Length - 1, 1);
                jsonForObj += "}";
            }
            catch (Exception ex)
            {
            }
            return jsonForObj;
        }

        public static string DecodeBase64String(string base64String)
        {
            string decodedString = string.Empty;
            try
            {
                byte[] data = Convert.FromBase64String(base64String);
                decodedString = Encoding.UTF8.GetString(data);
            }
            catch (Exception ex)
            {

            }
            return decodedString;
        }

        /// <summary>
        /// Method to convert Input amount from Rupee to Paise
        /// </summary>
        /// <param name="strAmount">
        /// Input Amount in rupee
        /// </param>
        /// <returns>
        /// Amount in Paise
        /// </returns>
        public long ConvertAmountToPaise(string strAmount)
        {
            try
            {
                double dbAmount = Convert.ToDouble(strAmount);
                return Convert.ToInt64(dbAmount * 100);
            }
            catch (Exception ex)
            {
            }
            return 0;
        }

        public int ConvertPercentToValue(string strValue)
        {
            try
            {
                double dbAmount = Convert.ToDouble(strValue);
                return Convert.ToInt32(dbAmount * 10000);
            }
            catch (Exception ex)
            {
            }
            return 0;
        }

        /// <summary>
        /// Method to convert Amount from Paise to Rupee
        /// </summary>
        /// <param name="lAmount">
        /// Amount in Paise
        /// </param>
        /// <returns>
        /// Amount in Rupee
        /// </returns>

        public string ConvertAmountToRupees(long lAmount)
        {
            long iQuotient = lAmount / 100;
            string strRemainder = Convert.ToString(lAmount % 100);
            strRemainder = strRemainder.PadLeft(2, '0');

            strRemainder = strRemainder.Substring(0, 2);
            string strAmount = iQuotient.ToString() + "." + strRemainder.ToString();
            return strAmount;
        }

        public string ConvertValueToPercent(int iPercentValue)
        {
            int iQuotient = iPercentValue / 10000;
            string strRemainder = Convert.ToString(iPercentValue % 10000);
            strRemainder = strRemainder.PadLeft(4, '0');

            strRemainder = strRemainder.Substring(0, 2);

            string strPercent = iQuotient.ToString() + "." + strRemainder.ToString();
            return strPercent;
        }

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


        public static OrderDetailsResponseSentToMerchant GetOrderDetailsResponseSentToMerchant(OrderDetailsDto orderDetailsDto)
        {
            OrderDetailsResponseSentToMerchant orderDetailsResponseSentToMerchant = new OrderDetailsResponseSentToMerchant();
            MerchantDetailsResponse merchantDetailsResponse = orderDetailsResponseSentToMerchant.MerchantDetailsResponse = new MerchantDetailsResponse();
            OrderDetailsResponse orderDetailsResponse = orderDetailsResponseSentToMerchant.OrderDetailsResponse = new OrderDetailsResponse();



            merchantDetailsResponse.MerchantId = orderDetailsDto.MerchantDto.MerchantId;
            merchantDetailsResponse.MerchantOrderID = orderDetailsDto.MerchantDto.MerchantOrderID;

            orderDetailsResponse.AggOrderID = orderDetailsDto.OrderTxnInfoDto.AggOrderId;
            orderDetailsResponse.Amount = orderDetailsDto.OrderTxnInfoDto.Amount;
            orderDetailsResponse.CurrencyCode = orderDetailsDto.OrderTxnInfoDto.CurrencyCode;
            orderDetailsResponse.OrderDesc = orderDetailsDto.OrderTxnInfoDto.OrderDesc;
            orderDetailsResponse.OrderResponseCode = orderDetailsDto.OrderTxnInfoDto.OrderResponseCode;
            orderDetailsResponse.OrderStatus = orderDetailsDto.OrderTxnInfoDto.OrderStatusCode;
            orderDetailsResponse.TxnType = orderDetailsDto.OrderTxnInfoDto.TxnTypeCode;

            if (orderDetailsDto.OrderTxnInfoDto.TxnTypeCode == EnumTxnType.REFUND)
            {
                orderDetailsResponse.ParentAggOrderId = orderDetailsDto.OrderTxnInfoDto.ParentOrderId;
                orderDetailsResponse.ParentOrderResponseCode = orderDetailsDto.OrderTxnInfoDto.ParentOrderResponseCode;
                orderDetailsResponse.ParentOrderStatus = orderDetailsDto.OrderTxnInfoDto.ParentOrderStatusCode;

            }
            else
            {
                orderDetailsResponse.RefundAmount = orderDetailsDto.OrderTxnInfoDto.RefundAmount;

            }

            if (orderDetailsDto.PaymentDataDto != null)
            {
                PaymentTxnResponse paymentTxnResponse = orderDetailsResponseSentToMerchant.PaymentTxnResponse = new PaymentTxnResponse();
                GatewayDetailsResponse gatewayDetailsResponse = paymentTxnResponse.GatewayDetailsResponse = new GatewayDetailsResponse();
                AcquirerDataResponse acquirerDataResponse = paymentTxnResponse.AcquirerDataResponse = new AcquirerDataResponse();
                if (orderDetailsDto.PaymentDataDto.PaymentMode > 0)
                {
                    paymentTxnResponse.PaymentMode = (EnumPaymentMode)orderDetailsDto.PaymentDataDto.PaymentMode;
                }

                gatewayDetailsResponse.GatewayCode = (EnumGateway)orderDetailsDto.PaymentDataDto.GatewayId;
                gatewayDetailsResponse.OrderId = orderDetailsDto.PaymentDataDto.OrderIdGeneratedByGateway;
                gatewayDetailsResponse.PaymentId = orderDetailsDto.PaymentDataDto.PaymentIdGeneratedByGateway;
                gatewayDetailsResponse.Status = orderDetailsDto.PaymentDataDto.Status;

                acquirerDataResponse.AcquirerName = String.Empty;

            }






            return orderDetailsResponseSentToMerchant;






        }

        public static int GetCurrentUnixTimeStamp()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static bool TryParseEnum<TEnum>( int enumValue, out TEnum retVal)
        {
            retVal = default(TEnum);
            bool success = Enum.IsDefined(typeof(TEnum), enumValue);
            if (success)
            {
                retVal = (TEnum)Enum.ToObject(typeof(TEnum), enumValue);
            }
            return success;
        }
    }
}