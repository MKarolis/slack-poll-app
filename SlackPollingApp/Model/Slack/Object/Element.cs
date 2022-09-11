using System.Collections.Generic;
using System.Text.Json.Serialization;
using SlackPollingApp.Model.Slack.Common;

namespace SlackPollingApp.Model.Slack.Object
{
    public class Element
    {
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("label")] public TextNode Label { get; set; }
        [JsonPropertyName("placeholder")] public TextNode Placeholder { get; set; }
        [JsonPropertyName("action_id")] public string ActionId { get; set; }
        [JsonPropertyName("max_length")] public int? MaxLength { get; set; }
        [JsonPropertyName("focus_on_load")] public bool? FocusOnLoad { get; set; }
        [JsonPropertyName("style")] public string Style { get; set; }
        [JsonPropertyName("text")] public TextNode Text { get; set; }
        [JsonPropertyName("options")] public List<Option> Options { get; set; }
        [JsonPropertyName("initial_options")] public List<Option> InitialOptions { get; set; }
    }
}