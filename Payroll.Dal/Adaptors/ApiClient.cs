using Newtonsoft.Json;
using Payroll.Dal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Reviews.Dal.Adaptors
{
    public class ApiClient : IApiClient
    {
        private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(90);
        protected HttpClient HttpClient;

        public ApiClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public ApiClient(string baseUrl, IEnumerable<KeyValuePair<string, string>> defaultHeaders)
        {
            HttpClient = new HttpClient();
            Init(baseUrl, defaultHeaders);
        }

        protected void Init(string baseUrl, IEnumerable<KeyValuePair<string, string>> defaultHeaders)
        {
            baseUrl = NormalizeBaseUrl(baseUrl);

            HttpClient.BaseAddress = new Uri(baseUrl);

            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive");

            HttpClient.Timeout = _timeout;

            if (defaultHeaders != null)
                HttpClient.AddDefaultHeaders(defaultHeaders);

        }

        public async Task<T> GetAsync<T>(string endpoint, IEnumerable<KeyValuePair<string, object>> parameters = null, IEnumerable<KeyValuePair<string, object>> headers = null)
        {
            endpoint = CreateUrl(endpoint, parameters);

            var httpResponse = await GetAsync(endpoint, headers);

            try
            {
                var result = await DeserializeContent<T>(httpResponse);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<T> PostAsync<T>(string endpoint, dynamic body, IEnumerable<KeyValuePair<string, object>> parameters = null, IEnumerable<KeyValuePair<string, object>> headers = null)
        {
            endpoint = CreateUrl(endpoint, parameters);
            var httpResponse = await PostAsync(endpoint, body, headers);

            var value = await DeserializeContent<T>(httpResponse);
            return value;
        }

        public async Task<T> PutAsync<T>(string endpoint, dynamic body, IEnumerable<KeyValuePair<string, object>> parameters = null, IEnumerable<KeyValuePair<string, object>> headers = null)
        {
            endpoint = CreateUrl(endpoint, parameters);
            var httpResponse = await PutAsync(endpoint, body, headers);

            return await DeserializeContent<T>(httpResponse);

        }

        public async Task<T> DeleteAsync<T>(string endpoint)
        {
            var httpResponse = await DeleteAsync(endpoint);

            return await DeserializeContent<T>(httpResponse);

        }

        private async Task<HttpResponseMessage> GetAsync(string url, IEnumerable<KeyValuePair<string, object>> headers)
        {
            HttpResponseMessage response;
            if (headers == null || !headers.Any())
            {
                response = await HttpClient.GetAsync(url);
                return response;
            }

            response = await HttpClient.SendAsync(url, HttpMethod.Get, null, headers);
            return response;
        }

        private async Task<HttpResponseMessage> PostAsync(string url, dynamic body, IEnumerable<KeyValuePair<string, object>> headers)
        {
            string json = ConvertToJsonString(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            if (headers == null || !headers.Any())
            {
                return await HttpClient.PostAsync(url, content);
            }

            return await HttpClient.SendAsync(url, HttpMethod.Post, content, headers);
        }

        private async Task<HttpResponseMessage> PutAsync(string url, dynamic body, IEnumerable<KeyValuePair<string, object>> headers)
        {
            string json = ConvertToJsonString(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            if (headers == null || !headers.Any())
            {
                return await HttpClient.PutAsync(url, content);
            }

            return await HttpClient.SendAsync(url, HttpMethod.Put, content, headers);
        }

        private async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await HttpClient.DeleteAsync(url);
        }

        private string CreateUrl(string endpoint, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            if (parameters == null || !parameters.Any())
                return endpoint;
            var queryStr = parameters.AsQueryString();
            if (endpoint.Contains("?"))
            {
                endpoint += queryStr;
            }
            else
            {
                endpoint += "?" + queryStr.TrimStart('&');
            }

            return endpoint;
        }

        private string ConvertToJsonString(object obj)
        {
            if (obj == null)
            {
                return "[]";
            }

            return JsonConvert.SerializeObject(obj);
        }

        private async Task<T> DeserializeContent<T>(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var strResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(strResponse);
            }
            else
            {
                throw new Exception();
            }
        }

        private string NormalizeBaseUrl(string url)
        {
            url = url.Trim();
            return url.EndsWith("/") ? url : url + "/";
        }

    }

    public static class HttpClientExtension
    {
        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, string uri, HttpMethod httpMethod, HttpContent httpContent, IEnumerable<KeyValuePair<string, object>> headers)
        {
            var httpRequestMessage = new HttpRequestMessage(httpMethod, uri);

            foreach (var kvp in headers)
            {
                httpRequestMessage.Headers.Add(kvp.Key, kvp.Value.ToString());
            }

            if (httpContent != null)
                httpRequestMessage.Content = httpContent;

            var result = await httpClient.SendAsync(httpRequestMessage);

            return result;
        }


        public static void AddDefaultHeaders(this HttpClient httpClient, IEnumerable<KeyValuePair<string, string>> headers)
        {
            foreach (var kvp in headers)
            {
                httpClient.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
            }
        }

        public static string AsQueryString(this IEnumerable<KeyValuePair<string, object>> parameters)
        {
            var builder = new StringBuilder();
            var separator = "&";
            foreach (var kvp in parameters.Where(kvp => kvp.Value != null))
            {
                if (kvp.Value.GetType().IsGenericType)
                {
                    List<string> itemList = (List<string>)kvp.Value;
                    foreach (var item in itemList)
                    {
                        builder.AppendFormat("{0}{1}={2}", separator, WebUtility.UrlEncode(kvp.Key), WebUtility.UrlEncode(item.ToString()));
                    }
                }
                else
                {
                    builder.AppendFormat("{0}{1}={2}", separator, WebUtility.UrlEncode(kvp.Key), WebUtility.UrlEncode(kvp.Value.ToString()));
                }
            }
            return builder.ToString();
        }

    }
}
