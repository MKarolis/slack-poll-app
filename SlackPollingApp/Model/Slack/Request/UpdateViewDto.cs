using System.Text.Json.Serialization;
using SlackPollingApp.Model.Slack.Object;

namespace SlackPollingApp.Model.Slack.Request
{
    public class UpdateViewDto
    {
        [JsonPropertyName("view_id")]
        public string ViewId { get; set; }
        [JsonPropertyName("view")]
        public View View { get; set; }
    }
}