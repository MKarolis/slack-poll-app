using System.Collections.Generic;
using System.Linq;
using SlackPollingApp.Model.Entity;
using SlackPollingApp.Model.Slack.Object;
using SlackPollingApp.Model.Slack.Object.Builder;
using SlackPollingApp.Model.Slack.Request;
using Option = SlackPollingApp.Model.Entity.Option;
using static SlackPollingApp.Model.Slack.Common.BlockBuildingConstants;

namespace SlackPollingApp.Model.Slack.Helper
{
    public static class MessageUiHelper
    {
        public static PostMessageDto ComposeInitialMessage(Poll poll)
        {
            var message = new PostMessageDto
            {
                Channel = poll.ChannelId,
                Blocks = new List<Block>
                {
                    new SectionBuilder().WithMarkdown(FormatMessageHeader(poll)).Build(),
                    DividerBuilder.BuildDivider(),
                }
            };

            AddOptionFields(poll, message.Blocks);

            return message;
        }

        public static List<Block> RebuildUpdatedMessageBlocks(Poll poll)
        {
            var blocks = new List<Block>
            {
                new SectionBuilder().WithMarkdown(FormatMessageHeader(poll)).Build(),
                DividerBuilder.BuildDivider(),
            };
            if (poll.Locked) AddFrozenIndicator(blocks);
            AddOptionFields(poll, blocks);

            return blocks;
        }

        private static void AddFrozenIndicator(List<Block> blocks)
        {
            blocks.Add(new SectionBuilder().WithMarkdown("Poll is _locked_ by owner, no votes can be changed").Build());
        }

        private static void AddOptionFields(Poll poll, List<Block> blocks)
        {
            var totalVoteCount = poll.Options.Sum(o => o.Votes.Count);
            foreach (var option in poll.Options)
            {
                var builder = new SectionBuilder().WithMarkdown(FormatOptionText(option, totalVoteCount));
                if (!poll.Locked) builder.WithButton("Vote", option.Value);
                blocks.Add(builder.Build());
                if (poll.ShowVoters && option.Votes.Any())
                {
                    var text = string.Join(", ", option.Votes.Select(vote => $"<@{vote.Id}>"));
                    blocks.Add(new SectionBuilder()
                        .WithMarkdown(text)
                        .Build());
                }
            }
        }

        private static string FormatOptionText(Option option, int totalVoteCount)
        {
            var votes = option.Votes.Count;
            var percentage = totalVoteCount > 0 ? (double) votes / totalVoteCount : 0;

            var progressBar = GetStatusBar(votes, percentage);
            return $"*{option.Title}*\n{progressBar} {(int) (percentage * 100)}% ({votes}/{totalVoteCount})";
        }

        private static string GetStatusBar(int votes, double percentage)
        {
            if (votes <= 0) return $"`|{new string(' ', ProgressBarLength - 1)}`";

            var filledLength = (int) (ProgressBarLength * percentage);
            return $"`{new string('#', filledLength)}{new string(' ', ProgressBarLength - filledLength)}`";
        }

        private static string FormatMessageHeader(Poll poll) => $"*{poll.Title}*\nPoll by <@{poll.Owner}>";
    }
}