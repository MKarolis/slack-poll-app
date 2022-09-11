using System.Text.Json.Serialization;

namespace SlackPollingApp.Model.Slack.Request
{
    public class DeleteMessageDto
    {
        [JsonPropertyName("channel")] public string Channel { get; set; }
        [JsonPropertyName("ts")] public string Ts { get; set; }
    }
}