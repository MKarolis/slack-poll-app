using System.Text.Json.Serialization;
using static SlackPollingApp.Model.Slack.Common.BlockBuildingConstants;

namespace SlackPollingApp.Model.Slack.Common
{
    public class TextNode
    {
        [JsonPropertyName("type")] public string Type { get; set; } = PlainText;
        [JsonPropertyName("text")] public string Text { get; set; }

        public static TextNode Plain(string text)
        {
            return new TextNode
            {
                Text = text
            };
        }

        public static TextNode Markdown(string text)
        {
            return new TextNode
            {
                Type = BlockBuildingConstants.Markdown,
                Text = text
            };
        }
    }
}