using System.Text.Json.Serialization;

namespace SlackPollingApp.Model.Slack.Data
{
    public class SlackUserDto
    {
        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("username")] public string Username { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("team_id")] public string TeamId { get; set; }
    }
}