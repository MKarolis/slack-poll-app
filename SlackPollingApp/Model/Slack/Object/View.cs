using System.Collections.Generic;
using System.Text.Json.Serialization;
using SlackPollingApp.Model.Slack.Common;
using SlackPollingApp.Model.Slack.Data;

namespace SlackPollingApp.Model.Slack.Object
{
    public class View
    {
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("callback_id")] public string CallBackId { get; set; }
        [JsonPropertyName("title")] public TextNode Title { get; set; }
        [JsonPropertyName("blocks")] public List<Block> Blocks { get; set; }
        [JsonPropertyName("submit")] public TextNode Submit { get; set; }
        [JsonPropertyName("private_metadata")] public string PrivateMetadata { get; set; }
        [JsonPropertyName("state")] public ViewState State { get; set; }
    }
}