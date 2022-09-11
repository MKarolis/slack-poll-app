using System.Collections.Generic;
using System.Text.Json.Serialization;
using SlackPollingApp.Model.Slack.Object;

namespace SlackPollingApp.Model.Slack.Data
{
    public class FieldStateDto
    {
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("value")] public string Value { get; set; }
        [JsonPropertyName("selected_options")] public List<Option> SelectedOptions { get; set; }
    }
}