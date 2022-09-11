using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SlackPollingApp.Model.Slack.Data
{
    public class ViewState
    {
        [JsonPropertyName("values")]
        public Dictionary<string, Dictionary<string, FieldStateDto>> Values { get; set; }
    }
}