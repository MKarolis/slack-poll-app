using System.Text.Json.Serialization;

namespace SlackPollingApp.Model.Auth
{
    public class AuthedUserDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}