using SlackPollingApp.Model.Slack.Data;

namespace SlackPollingApp.Business.Service.ActionHandler.Factory
{
    public interface IActionHandlerFactory
    {
        IActionHandler GetHandler(SlackInteractionDto slackInteractionDto);
    }
}