using System.Text.Json.Serialization;

namespace SlackPollingApp.Model.Slack.Data
{
    public class MessageCreatedDto
    {
        [JsonPropertyName("ok")] public bool Ok { get; set; }
        [JsonPropertyName("ts")] public string Ts { get; set; }
    }
}