using System.Threading.Tasks;
using SlackPollingApp.Model.Slack.Data;

namespace SlackPollingApp.Business.Service.ActionHandler
{
    public interface IActionHandler
    {
        Task Handle(SlackInteractionDto interactionDto);
    }
}