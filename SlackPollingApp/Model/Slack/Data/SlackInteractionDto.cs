using System.Collections.Generic;
using System.Text.Json.Serialization;
using SlackPollingApp.Model.Slack.Object;

namespace SlackPollingApp.Model.Slack.Data
{
    public class SlackInteractionDto
    {
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("user")] public SlackUserDto User { get; set; }
        [JsonPropertyName("container")] public SlackContainerDto Container { get; set; }
        [JsonPropertyName("trigger_id")] public string TriggerId { get; set; }
        [JsonPropertyName("view")] public View View { get; set; }
        [JsonPropertyName("actions")] public List<SlackActionDto> Actions { get; set; }
        [JsonPropertyName("response_url")] public string ResponseUrl { get; set; }
    }
}