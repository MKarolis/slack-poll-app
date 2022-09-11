using System.Text.Json.Serialization;

namespace SlackPollingApp.Model.Slack.Data
{
    public class SlackContainerDto
    {
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("view_id")] public string ViewId { get; set; }
        [JsonPropertyName("message_ts")] public string MessageTs { get; set; }
        [JsonPropertyName("channel_id")] public string ChannelId { get; set; }
    }
}