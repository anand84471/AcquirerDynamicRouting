using System;
using System.ServiceModel;

namespace ExpressCheckoutDb.Extensions
{
    public static class ExpressCheckoutDBServiceClientExtension
    {
        public static TReturn DoDBCall<TClient, TReturn>(this TClient client, Func<TReturn> codeToBeExecuted)
                where TClient : class, ICommunicationObject

        {
            TReturn result;
            bool success = false;
            try
            {
                result = codeToBeExecuted();
                client.Close();
                success = true;
            }
            finally
            {
                if (!success && client != null)
                {
                    client.Abort();
                }
            }
            return result;
        }
    }
}