using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SlackPollingApp.Core.Config;

namespace SlackPollingApp.Core.Http
{
    public class HttpRequestSender : IHttpRequestSender
    {
        private static readonly JsonSerializerOptions DefaultJsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly SlackConfig _slackConfig;

        public HttpRequestSender(IHttpClientFactory httpClientFactory, IOptions<SlackConfig> slackOptions)
        {
            _httpClientFactory = httpClientFactory;
            _slackConfig = slackOptions.Value;
        }

        public async Task<string> PostAsync(string url, object body)
        {
            var httpClient = GetClient();
            var bodyJson = JsonSerializer.Serialize(body, DefaultJsonOptions);
            Console.WriteLine(bodyJson);

            var requestContent = new StringContent(bodyJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, requestContent);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private HttpClient GetClient()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _slackConfig.BotToken);

            return httpClient;
        }
    }
}