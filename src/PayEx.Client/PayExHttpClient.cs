using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using PayEx.Client.Exceptions;
using PayEx.Client.Models.Vipps;

namespace PayEx.Client
{
    public class PayExHttpClient
    {
        private readonly HttpClient _client;
        private readonly ILogPayExHttpResponse _logger;

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public PayExHttpClient(HttpClient client, ILogPayExHttpResponse logger)
        {
            _client = client;
            _logger = logger;
        }

        internal async Task<TY> HttpPost<T, TY>(string url, Func<ProblemsContainer, Exception> onError, T payload)
        {
            return await HttpRequest<TY>("POST", url, onError, payload);
        }

        internal async Task<TY> HttpPatch<T, TY>(string url, Func<ProblemsContainer, Exception> onError, T payload)
        {
            return await HttpRequest<TY>("PATCH", url, onError, payload);
        }

        internal async Task<T> HttpGet<T>(string url,
            Func<ProblemsContainer, Exception> onError)
        {
            return await HttpRequest<T>("GET", url, onError);
        }

        private async Task<T> HttpRequest<T>(string httpMethod, string url, Func<ProblemsContainer, Exception> onError, object payload = null)
        {
            var msg = new HttpRequestMessage(new HttpMethod(httpMethod), url);
            
            if (payload != null)
            {
                var content = JsonConvert.SerializeObject(payload, Settings);
                msg.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }
            msg.Headers.Add("Accept", "application/json");


            HttpResponseMessage response;
            try
            {
                response = await _client.SendAsync(msg);
            }
            catch (HttpRequestException e)
            {
                throw new BadRequestException(e);
            }
            catch (TaskCanceledException te)
            {
                throw new ApiTimeOutException(te);
            }

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                _logger.LogPayExResponse(res);
                return JsonConvert.DeserializeObject<T>(res, Settings);
            }

            var responseMessage = await response.Content.ReadAsStringAsync();
            _logger.LogPayExResponse(responseMessage);
            ProblemsContainer problems;
            if (!string.IsNullOrEmpty(responseMessage) && IsValidJson(responseMessage))
            {
                problems = JsonConvert.DeserializeObject<ProblemsContainer>(responseMessage);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                problems = new ProblemsContainer("id", "Not found");
            }
            else
            {
                IEnumerable<string> customHeader = null;
                _client.DefaultRequestHeaders.TryGetValues("X-Payex-ClientName", out customHeader);
                var aggr = customHeader != null? customHeader.Aggregate((x, y) => x + "," + y) : "no-name";
                problems = new ProblemsContainer("Other", $"Response when calling PayEx `{aggr}` was: '{response.StatusCode}'");
            }

            var ex = onError(problems);
            throw ex;
        }

        public async Task<string> GetRaw(string url)
        {
            var msg = new HttpRequestMessage(HttpMethod.Get, url);
            msg.Headers.Add("Accept", "application/json");


            HttpResponseMessage response;
            try
            {
                response = await _client.SendAsync(msg);
            }
            catch (HttpRequestException e)
            {
                throw new BadRequestException(e);
            }
            catch (TaskCanceledException te)
            {
                throw new ApiTimeOutException(te);
            }

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                var res2 = JToken.Parse(res).ToString(Formatting.Indented);
                _logger.LogPayExResponse(res2);
                return res2;
            }

            return null;
        }

        private static bool IsValidJson(string responseString)
        {
            responseString = responseString.Trim();
            if ((responseString.StartsWith("{") && responseString.EndsWith("}")) || (responseString.StartsWith("[") && responseString.EndsWith("]")))
            {
                try
                {
                    JToken.Parse(responseString);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
