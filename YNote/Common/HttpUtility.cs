using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace YNote.Common
{
    class HttpUtility
    {
        /// <summary>
        /// <parameter name="uri"> ?a=1&b=2 </parameter>
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetQueryParameters(string uri)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();

            int index = uri.LastIndexOf('?');
            if (index != -1)
            {
                uri.Remove(0, index+1);
            }

            if (!string.IsNullOrEmpty(uri))
            {
                string[] pairs = uri.Split('&');
                foreach (string s in pairs)
                {
                    string[] pair = s.Split('=');
                    if (pair.Length == 2)
                    {
                        dict.Add(pair[0], pair[1]);
                    }
                }
            }

            return dict;
        }

        public static string ToQueryStringForOAuthHeaders(IDictionary<string, string> dictionary)
        {
            var sb = new StringBuilder();
            sb.Append("OAuth");
            foreach (var key in dictionary.Keys)
            {
                var value = dictionary[key];
                if (value != null)
                {
                    sb.Append(" " + key + "=" + "\"" + value + "\"" + ",");
                }
            }
            return sb.ToString().TrimEnd(',');
        }

        public static string ToQueryString(IDictionary<string, string> dictionary)
        {
            var sb = new StringBuilder();
            foreach (var key in dictionary.Keys)
            {
                var value = dictionary[key];
                if (value != null)
                {
                    sb.Append(key + "=" + value + "&");
                }
            }
            return sb.ToString().TrimEnd('&');
        }

        /// <summary>
        /// This is a different Url Encode implementation since the default .NET one outputs the percent encoding in lower case.
        /// While this is not a problem with the percent encoding spec, it is used in upper case throughout OAuth
        /// </summary>
        /// <param name="value">The value to Url encode</param>
        /// <returns>Returns a Url encoded string</returns>
        private static string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
        public static string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();

            foreach (char symbol in value)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Get http result
        /// </summary>
        /// <param name="url"> uri </param>
        /// <returns></returns>
        public static async Task<string> HttpGet(string url)
        {
            string result = string.Empty;
            HttpClient httpClient = new HttpClient();
            try
            {
                HttpResponseMessage responseMsg = await httpClient.GetAsync(url);
                responseMsg.EnsureSuccessStatusCode();
                result = await responseMsg.Content.ReadAsStringAsync();
            }
            catch (System.Exception ex)
            {
                string error = ex.Message;
            }

            return result;
        }

        public static async Task<string> HttpGet(string url, IDictionary<string, string> headers)
        {
            string result = string.Empty;
            try
            {
                HttpClient httpClient = new HttpClient();
                foreach (var s in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(s.Key, s.Value);
                }
                result = await httpClient.GetStringAsync(url);
            }
            catch (System.Exception ex)
            {
                string error = ex.Message;
            }

            return result;
        }

        public static async Task<string> HttpPost(string url, IDictionary<string, string> headers, string content)
        {
            string result = string.Empty;
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpContent httpContent = new StringContent(content);
                foreach (var s in headers)
                {
                    if (httpContent.Headers.Contains(s.Key))
                    {
                        httpContent.Headers.Remove(s.Key);
                    }
                    httpContent.Headers.Add(s.Key, s.Value);
                }

                HttpResponseMessage responseMsg = await httpClient.PostAsync(url, httpContent);
                responseMsg.EnsureSuccessStatusCode();
                result = await responseMsg.Content.ReadAsStringAsync();
            }
            catch (System.Exception ex)
            {
                string error = ex.Message;
            }

            return result;
        }

        public static async Task<string> HttpPost(string url, string content)
        {
            string result = string.Empty;
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage responseMsg = await httpClient.PostAsync(url, new StringContent(content));
                responseMsg.EnsureSuccessStatusCode();
                result = await responseMsg.Content.ReadAsStringAsync();
            }
            catch (System.Exception ex)
            {
                string error = ex.Message;
            }

            return result;
        }
    }
}
