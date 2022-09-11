using System.Text.Json.Serialization;
using SlackPollingApp.Model.Slack.Common;

namespace SlackPollingApp.Model.Slack.Object
{
    public class Option
    {
        [JsonPropertyName("text")] public TextNode Text { get; set; }
        [JsonPropertyName("value")] public string Value { get; set; }
    }
}