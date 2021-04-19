using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;

namespace Leaderboard
{
    public static class HttpHelper
    {
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public static async Task<T> GetAsync<T>(this HttpClient httpClient, string url)
        {
            CheckIsNotNull(nameof(httpClient), httpClient);
            CheckIsWellFormedUri(nameof(url), url, UriKind.Absolute);

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static async Task SendAsync<T>(this HttpClient httpClient, string url, T sendThis)
        {
            CheckIsNotNull(nameof(httpClient), httpClient);
            CheckIsWellFormedUri(nameof(url), url, UriKind.Absolute);

            var sendThisAsString = Serialize(sendThis);

            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                using (var stringContent = new StringContent(sendThisAsString, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;
                    var response = await httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        public static async Task<V> SendAsync<T, V>(this HttpClient httpClient, string url, T sendThis)
        {
            CheckIsNotNull(nameof(httpClient), httpClient);
            CheckIsWellFormedUri(nameof(url), url, UriKind.Absolute);

            var sendThisAsString = Serialize(sendThis);

            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                using (var stringContent = new StringContent(sendThisAsString, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    using (var response = await httpClient
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                        .ConfigureAwait(false))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception(response.ReasonPhrase);
                        }
                        else
                        {
                            response.EnsureSuccessStatusCode();
                            var responseAsString = await response.Content.ReadAsStringAsync();
                            return Deserialize<V>(responseAsString);
                        }
                    }
                }
            }
        }

        public static string Serialize<T>(T serializeThis)
        {
            CheckIsNotNull(nameof(serializeThis), serializeThis);

            return JsonConvert.SerializeObject(serializeThis, Formatting.Indented, _jsonSettings);
        }

        public static T Deserialize<T>(string deserializeThis)
        {
            CheckIsNotNullOrWhitespace(nameof(deserializeThis), deserializeThis);

            return JsonConvert.DeserializeObject<T>(deserializeThis, _jsonSettings);
        }
    }
}
