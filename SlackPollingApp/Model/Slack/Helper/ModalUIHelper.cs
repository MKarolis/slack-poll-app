using System.Collections.Generic;
using SlackPollingApp.Model.Slack.Common;
using SlackPollingApp.Model.Slack.Object;
using SlackPollingApp.Model.Slack.Object.Builder;

using static SlackPollingApp.Model.Slack.Common.BlockBuildingConstants;

namespace SlackPollingApp.Model.Slack.Helper
{
    public static class ModalUiHelper
    {
        public static View BuildWarningModal(string message)
        {
            return new View
            {
                Type = ViewTypeModal,
                Title = TextNode.Plain("Warning"),
                Blocks = new List<Block>
                {
                    new SectionBuilder()
                        .WithPlainText(message)
                        .Build()
                }
            };
        }
        
        public static View BuildInitialModal()
        {
            return new View
            {
                Type = ViewTypeModal,
                Title = TextNode.Plain("Create a Poll"),
                Blocks = new List<Block>
                {
                    new InputBuilder(ActionIdQuestion)
                        .WithPlainInput()
                        .WithLabel("Poll Question")
                        .Build(),
                    new InputBuilder(GetOptionActionId(1))
                        .WithPlainInput()
                        .WithLabel("Option 1")
                        .Build(),
                    new InputBuilder(GetOptionActionId(2))
                        .WithPlainInput()
                        .WithLabel("Option 2")
                        .Build(),
                    new ActionsBuilder()
                        .WithRemoveButton()
                        .WithAddButton()
                        .Build(),
                    new InputBuilder(ActionIdCheckboxChange)
                        .WithCheckboxes()
                        .WithOption("Allow users to select multiple options", OptionValueMultipleOptions)
                        .WithOption("Show voters", OptionValueShowVoters)
                        .WithLabel("Poll Options")
                        .Build()
                },
                Submit = TextNode.Plain("Create a Poll")
            };
        }

        public static void AddQuestionOption(View modal)
        {
            var optionsCount = modal.Blocks
                .FindAll(b => b.Type == BlockTypeInput && b.Element.ActionId.StartsWith(ActionIdOptionPrefix))
                .Count;
            var actionsIndex = modal.Blocks.FindIndex(b => b.Type == BlockTypeActions);

            if (optionsCount >= MaxOptionsCount - 1)
            {
                modal.Blocks[actionsIndex] = new ActionsBuilder().WithRemoveButton().Build();
            }
            else if (modal.Blocks[actionsIndex].Elements.Count < 2)
            {
                modal.Blocks[actionsIndex] = new ActionsBuilder().WithRemoveButton().WithAddButton().Build();
            }

            if (optionsCount < MaxOptionsCount)
                modal.Blocks.Insert(actionsIndex, new InputBuilder(GetOptionActionId(optionsCount + 1))
                    .WithPlainInput()
                    .WithLabel($"Option {optionsCount + 1}")
                    .Build());
        }

        public static void RemoveQuestionOption(View modal)
        {
            var optionsCount = modal.Blocks
                .FindAll(b => b.Type == BlockTypeInput && b.Element.ActionId.StartsWith(ActionIdOptionPrefix))
                .Count;
            var actionsIndex = modal.Blocks.FindIndex(b => b.Type == BlockTypeActions);

            if (optionsCount <= MinOptionsCount + 1)
            {
                modal.Blocks[actionsIndex] = new ActionsBuilder().WithAddButton().Build();
            }
            else if (modal.Blocks[actionsIndex].Elements.Count < 2)
            {
                modal.Blocks[actionsIndex] = new ActionsBuilder().WithRemoveButton().WithAddButton().Build();
            }

            if (optionsCount > MinOptionsCount) modal.Blocks.RemoveAt(actionsIndex - 1);
        }
    }
}