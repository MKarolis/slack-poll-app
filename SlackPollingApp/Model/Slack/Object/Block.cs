using System.Collections.Generic;
using System.Text.Json.Serialization;
using SlackPollingApp.Model.Slack.Common;

namespace SlackPollingApp.Model.Slack.Object
{
    public class Block
    {
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("block_id")] public string BlockId { get; set; }
        [JsonPropertyName("text")] public TextNode Text { get; set; }
        [JsonPropertyName("label")] public TextNode Label { get; set; }
        [JsonPropertyName("element")] public Element Element { get; set; }
        [JsonPropertyName("accessory")] public Element Accessory { get; set; }
        [JsonPropertyName("elements")] public List<Element> Elements { get; set; }
        [JsonPropertyName("optional")] public bool? Optional { get; set; }
    }
}