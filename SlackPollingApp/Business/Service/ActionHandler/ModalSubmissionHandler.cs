using System.Text.Json;
using System.Threading.Tasks;
using SlackPollingApp.Core.Http;
using SlackPollingApp.Model.Slack.Common;
using SlackPollingApp.Model.Slack.Data;
using SlackPollingApp.Model.Slack.Helper;

namespace SlackPollingApp.Business.Service.ActionHandler
{
    public class ModalSubmissionHandler : IActionHandler
    {
        private readonly IHttpRequestSender _httpRequestSender;
        private readonly IPollService _pollService;
        private readonly INotificationService _notificationService;

        public ModalSubmissionHandler(
            IHttpRequestSender httpRequestSender, IPollService pollService, INotificationService notificationService)
        {
            _httpRequestSender = httpRequestSender;
            _pollService = pollService;
            _notificationService = notificationService;
        }
        
        public async Task Handle(SlackInteractionDto interactionDto)
        {
            var poll = await _pollService.CreatePoll(interactionDto);

            var messageDto = MessageUiHelper.ComposeInitialMessage(poll);
            var result = await _httpRequestSender.PostToSlackByPathAsync(CommonSlackPaths.PostMessagePath, messageDto);
            var resultDto = JsonSerializer.Deserialize<MessageCreatedDto>(result);

            await _pollService.UpdatePollTimestamp(poll.Id, resultDto?.Ts);
            await _notificationService.PublishPollCreated(poll.Owner);
        }
    }
}