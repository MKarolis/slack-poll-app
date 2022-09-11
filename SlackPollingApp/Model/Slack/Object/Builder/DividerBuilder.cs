
using static SlackPollingApp.Model.Slack.Common.BlockBuildingConstants;

namespace SlackPollingApp.Model.Slack.Object.Builder
{
    public static class DividerBuilder
    {
        public static Block BuildDivider()
        {
            return new Block
            {
                Type = BlockTypeDivider
            };
        }
    }
}