using Core.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Core.Utilities
{
    public class GenericCoreUtility
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


        public static string CreateFormToPost(string url, NameValueCollection nv)
        {
            string formId = "__PostForm";
            
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<html> <head><meta charset=\"utf-8\"> <meta name=\"viewport\" content=\"width=device-width\"> </head>");
            strForm.Append(string.Format("<form id=\"{0}\" name=\"{0}\" action=\"{1}\" method=\"POST\">", formId, url));
            for (int i = 0; i < nv.Keys.Count; i++)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + nv.Keys[i] + "\" value=\"" + nv[nv.Keys[i]] + "\">");
            }
            
            strForm.Append("</form>");
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language=\"javascript\">");
            strScript.Append(string.Format("var v{0}=document.{0};", formId));
            strScript.Append(string.Format("v{0}.submit();", formId));
            strScript.Append("</script>");
            return strForm.ToString() + strScript.ToString() + "</html>";
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
    }
}