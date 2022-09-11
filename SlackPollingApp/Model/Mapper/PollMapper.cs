using System;
using System.Collections.Generic;
using System.Linq;
using SlackPollingApp.Business.Helper;
using SlackPollingApp.Model.Entity;
using SlackPollingApp.Model.Slack.Data;
using static SlackPollingApp.Model.Slack.Common.BlockBuildingConstants;

namespace SlackPollingApp.Model.Mapper
{
    public static class PollMapper
    {
        public static Poll map(SlackInteractionDto interactionDto)
        {
            var fieldValuesById = interactionDto.View.State.Values.Values
                .SelectMany(entry => entry)
                .ToDictionary(entry => entry.Key, x => x.Value);

            return new Poll
            {
                ChannelId = MetadataHelper.GetChannelId(interactionDto.View.PrivateMetadata),
                Title = fieldValuesById[ActionIdQuestion].Value,
                Owner = interactionDto.User.Id,
                Options = fieldValuesById
                    .Where(entry => entry.Key.StartsWith(ActionIdOptionPrefix))
                    .Select(entry => new Option
                    {
                        Title = entry.Value.Value,
                        Value = entry.Key,
                        Votes = new List<UserSummary>()
                    }).ToList(),
                Date = DateTime.Now,
                ShowVoters = fieldValuesById[ActionIdCheckboxes].SelectedOptions
                    .Any(o => o.Value == OptionValueShowVoters),
                MultiSelect = fieldValuesById[ActionIdCheckboxes].SelectedOptions
                    .Any(o => o.Value == OptionValueMultipleOptions)
            };
        }
    }
}