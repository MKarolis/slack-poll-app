using System.Collections.Generic;
using System.Text.Json.Serialization;
using SlackPollingApp.Model.Slack.Object;

namespace SlackPollingApp.Model.Slack.Request
{
    public class PostMessageDto
    {
        [JsonPropertyName("channel")] public string Channel { get; set; }
        [JsonPropertyName("text")] public string Text { get; set; }
        [JsonPropertyName("blocks")] public List<Block> Blocks { get; set; }
        [JsonPropertyName("replace_original")] public bool? ReplaceOriginal { get; set; }
        [JsonPropertyName("response_type")] public string ResponseType { get; set; }
        [JsonPropertyName("ts")] public string Ts { get; set; }
    }
}