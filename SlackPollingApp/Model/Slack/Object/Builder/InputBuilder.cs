using System.Collections.Generic;
using SlackPollingApp.Model.Slack.Common;
using static SlackPollingApp.Model.Slack.Common.BlockBuildingConstants;

namespace SlackPollingApp.Model.Slack.Object.Builder
{
    public class InputBuilder
    {
        private readonly Block _input;
        private readonly string _actionId;

        public InputBuilder(string actionId)
        {
            _actionId = actionId;
            _input = new Block
            {
                Type = BlockTypeInput,
                Label = TextNode.Plain(""),
            };
        }

        public InputBuilder WithPlainInput()
        {
            _input.Element = new Element
            {
                Type = ElementTypePlainTextInput,
                ActionId = _actionId,
                MaxLength = InputMaxLength,
                FocusOnLoad = false,
                Placeholder = TextNode.Plain(InputPlaceholder)
            };
            _input.Optional = false;
            return this;
        }

        public InputBuilder WithCheckboxes()
        {
            _input.Element = new Element
            {
                Type = ElementTypeCheckboxes,
                Options = new List<Option>(),
                InitialOptions = new List<Option>(),
                ActionId = ActionIdCheckboxes
            };
            _input.Optional = true;
            return this;
        }

        public InputBuilder WithOption(string label, string value, bool initial = true)
        {
            var option = new Option
            {
                Text = TextNode.Plain(label),
                Value = value
            };
            _input.Element.Options.Add(option);
            if (initial)
            {
                _input.Element.InitialOptions.Add(option);
            }
            return this;
        }

        public InputBuilder WithLabel(string label)
        {
            _input.Label.Text = label;
            return this;
        }

        public InputBuilder WithPlaceholder(string placeholder)
        {
            _input.Element.Placeholder = TextNode.Plain(placeholder);
            return this;
        }

        public InputBuilder Optional()
        {
            _input.Optional = true;
            return this;
        }

        public Block Build()
        {
            return _input;
        }
    }
}