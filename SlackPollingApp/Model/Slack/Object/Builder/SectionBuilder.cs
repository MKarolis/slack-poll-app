
using SlackPollingApp.Model.Slack.Common;
using static SlackPollingApp.Model.Slack.Common.BlockBuildingConstants;

namespace SlackPollingApp.Model.Slack.Object.Builder
{
    public class SectionBuilder
    {
        private readonly Block _block;

        public SectionBuilder()
        {
            _block = new Block
            {
                Type = BlockTypeSection,
            };
        }
        
        public SectionBuilder WithPlainText(string text)
        {
            _block.Text = TextNode.Plain(text);
            return this;
        }
        
        public SectionBuilder WithMarkdown(string text)
        {
            _block.Text = TextNode.Markdown(text);
            return this;
        }

        public SectionBuilder WithButton(string title, string actionId)
        {
            _block.Accessory = new Element
            {
                Type = ElementTypeButton,
                Text = TextNode.Plain(title),
                ActionId = actionId
            };
            return this;
        }

        public Block Build()
        {
            return _block;
        }
    }
}