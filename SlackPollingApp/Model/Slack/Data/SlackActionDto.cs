using System.Text.Json.Serialization;
using SlackPollingApp.Model.Slack.Common;

namespace SlackPollingApp.Model.Slack.Data
{
    public class SlackActionDto
    {
        [JsonPropertyName("action_id")] public string ActionId { get; set; }
        [JsonPropertyName("block_id")] public string BlockId { get; set; }
        [JsonPropertyName("text")] public TextNode Text { get; set; }
        [JsonPropertyName("style")] public string Style { get; set; }
        [JsonPropertyName("tape")] public string Tape { get; set; }
    }
}