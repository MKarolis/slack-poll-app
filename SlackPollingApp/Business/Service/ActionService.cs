using System.Threading.Tasks;
using SlackPollingApp.Business.Service.ActionHandler.Factory;
using SlackPollingApp.Model.Slack.Data;

namespace SlackPollingApp.Business.Service
{
    public class ActionService
    {
        private readonly IActionHandlerFactory _actionHandlerFactory;

        public ActionService(IActionHandlerFactory actionHandlerFactory)
        {
            _actionHandlerFactory = actionHandlerFactory;
        }

        public async Task HandleIncomingAction(SlackInteractionDto interactionDto)
        {
            await _actionHandlerFactory.GetHandler(interactionDto).Handle(interactionDto);
        }
    }
}