using SlackPollingApp.Core.Http;
using SlackPollingApp.Model.Slack.Common;
using SlackPollingApp.Model.Slack.Data;

namespace SlackPollingApp.Business.Service.ActionHandler.Factory
{
    public class ActionHandlerFactory : IActionHandlerFactory
    {
        private readonly IHttpRequestSender _httpRequestSender;
        private readonly IPollService _pollService;
        private readonly INotificationService _notificationService;

        public ActionHandlerFactory(
            IHttpRequestSender httpRequestSender, IPollService pollService, INotificationService notificationService)
        {
            _httpRequestSender = httpRequestSender;
            _pollService = pollService;
            _notificationService = notificationService;
        }

        public IActionHandler GetHandler(SlackInteractionDto slackInteractionDto)
        {
            if (slackInteractionDto.Type == ActionConstants.ActionTypeViewSubmission)
            {
                return new ModalSubmissionHandler(_httpRequestSender, _pollService, _notificationService);
            }

            return slackInteractionDto.Container.Type == ActionConstants.ContainerTypeView
                ? new ModalBlockActionHandler(_httpRequestSender)
                : new MessageBlockActionHandler(_httpRequestSender, _pollService, _notificationService);
        }
    }
}