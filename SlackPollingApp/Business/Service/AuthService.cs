using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SlackPollingApp.Model.Auth;

namespace SlackPollingApp.Business.Service
{
    public class AuthService
    {

        private readonly IHttpClientFactory _httpClientFactory;


        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SlackTokenResponse> GetAccessToken(string token)
        {
            var formData = new Dictionary<string, string>
            {
                {"client_id", "3474826765047.3489450726482"},
                {"client_secret", "8ac48800bb84041aca8c0c3a59822cbc"},
                {"code", token},
                {"redirect_uri", "https://localhost:5001/slack-callback"}
            };

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync("https://slack.com/api/oauth.v2.access", new FormUrlEncodedContent(formData));

            var responseData = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<SlackTokenResponse>(responseData);
        }
    }
}
