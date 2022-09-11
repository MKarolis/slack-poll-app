using System.Text.Json.Serialization;
using SlackPollingApp.Model.Slack.Object;

namespace SlackPollingApp.Model.Slack.Request
{
    public class ShowViewDto
    {
        [JsonPropertyName("trigger_id")]
        public string TriggerId { get; set; }
        [JsonPropertyName("view")]
        public View View { get; set; }
    }
}