using Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Core.ApiClients
{
    public class CoreApiClient
    {
        public static async Task<TResponse> DoGetAsync<THttpClient, TResponse>(THttpClient client, string url,string apiUsername,string apiPassword)
            where THttpClient : HttpClient
            where TResponse : class
        {
            TResponse response = null;
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", apiUsername, apiPassword))));
                HttpResponseMessage reponseMSg = await client.GetAsync(url);
                if (reponseMSg.IsSuccessStatusCode)
                {
                    string Data = reponseMSg.Content.ReadAsStringAsync().Result;
                    response = JsonConvert.DeserializeObject<TResponse>(Data);
                }
            }
            catch (HttpRequestException htpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public static async Task<TResponse> DoPostRequestJsonAsync<THttpClient, TRequest, TResponse>(THttpClient client, string url, TRequest request)
          where THttpClient : HttpClient
          where TResponse : class
          where TRequest : class
        {
            TResponse response = null;
            try
            {
                string stringData = JsonConvert.SerializeObject(request);
                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage reponseMSg = await client.PostAsync(url, contentData);
                if (reponseMSg.IsSuccessStatusCode)
                {
                    string Data = await reponseMSg.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<TResponse>(Data);
                }
            }
            catch (HttpRequestException htpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public static async Task<TResponse> DoPostRequestJsonAsyncWithCredential<THttpClient, TRequest, TResponse>
            (THttpClient client, string url, TRequest request, string apiUsername, string apiPassword)
            where THttpClient : HttpClient
            where TResponse : class
             where TRequest : class
        {
            TResponse response = null;
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", apiUsername, apiPassword))));
                string stringData = JsonConvert.SerializeObject(request);
                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage reponseMSg = await client.PostAsync(url, contentData);
                if (reponseMSg.IsSuccessStatusCode)
                {
                    string Data = await reponseMSg.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<TResponse>(Data);
                }
            }
            catch (HttpRequestException htpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public static async Task<string> DoPostRequestJsonAndGetStringAsync<THttpClient, TRequest>(THttpClient client, string url, TRequest request)
          where THttpClient : HttpClient
          where TRequest : class
        {
            string Data = string.Empty;
            try
            {
                
                string stringData = JsonConvert.SerializeObject(request);

                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage reponseMSg = await client.PostAsync(url, contentData);
                if (reponseMSg.IsSuccessStatusCode)
                {
                    Data = await reponseMSg.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException htpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

            return Data;
        }


        public static async Task<TResponse> DoPostFormUrlEncodedDataAsync<THttpClient, TRequest, TResponse>(THttpClient client, string url, TRequest request, TResponse response)
         where THttpClient : HttpClient
         where TResponse : class
         where TRequest : NameValueCollection
        {
            try
            {
                //string stringData = JsonConvert.SerializeObject(request);
                string formUrlEncodedQueryStringData = GenericCoreUtility.toQueryString(request);
                var contentData = new StringContent(formUrlEncodedQueryStringData, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
                HttpResponseMessage reponseMSg = await client.PostAsync(url, contentData);
                if (reponseMSg.IsSuccessStatusCode)
                {
                    string Data = reponseMSg.Content.ReadAsStringAsync().Result;
                    response = JsonConvert.DeserializeObject<TResponse>(Data);
                }
            }
            catch (HttpRequestException htpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
    }
}