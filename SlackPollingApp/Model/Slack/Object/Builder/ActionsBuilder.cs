using System.Collections.Generic;
using SlackPollingApp.Model.Slack.Common;

using static SlackPollingApp.Model.Slack.Common.BlockBuildingConstants;

namespace SlackPollingApp.Model.Slack.Object.Builder
{
    public class ActionsBuilder
    {
        private static readonly Element RemoveOptionButton = new Element
        {
            Type = ElementTypeButton,
            Text = TextNode.Plain("Remove Last Option"),
            ActionId = ActionIdRemoveOption,
            Style = StyleDanger
        };
        
        private static readonly Element AddOptionButton = new Element
        {
            Type = ElementTypeButton,
            Text = TextNode.Plain("Add Option"),
            ActionId = ActionIdAddOption,
            Style = StylePrimary
        };
        
        private readonly Block _block;

        public ActionsBuilder()
        {
            _block = new Block
            {
                Type = BlockTypeActions,
                Elements = new List<Element>()
            };
        }

        public ActionsBuilder WithRemoveButton()
        {
            _block.Elements.Add(RemoveOptionButton);
            return this;
        }
        
        public ActionsBuilder WithAddButton()
        {
            _block.Elements.Add(AddOptionButton);
            return this;
        }

        public Block Build()
        {
            return _block;
        }
    }
}