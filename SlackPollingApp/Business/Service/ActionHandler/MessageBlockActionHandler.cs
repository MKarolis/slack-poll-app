using System.Threading.Tasks;
using SlackPollingApp.Business.Exception;
using SlackPollingApp.Core.Http;
using SlackPollingApp.Model.Slack.Common;
using SlackPollingApp.Model.Slack.Data;
using SlackPollingApp.Model.Slack.Helper;
using SlackPollingApp.Model.Slack.Request;

namespace SlackPollingApp.Business.Service.ActionHandler
{
    public class MessageBlockActionHandler : IActionHandler
    {
        private readonly IHttpRequestSender _httpRequestSender;
        private readonly IPollService _pollService;
        private readonly INotificationService _notificationService;

        public MessageBlockActionHandler(
            IHttpRequestSender httpRequestSender, IPollService pollService, INotificationService notificationService)
        {
            _httpRequestSender = httpRequestSender;
            _pollService = pollService;
            _notificationService = notificationService;
        }
        
        public async Task Handle(SlackInteractionDto interactionDto)
        {
            try
            {
                var updatedPoll = await _pollService.ToggleVote(interactionDto);
                var updatedMsg = new PostMessageDto
                {
                    ReplaceOriginal = true,
                    Blocks = MessageUiHelper.RebuildUpdatedMessageBlocks(updatedPoll)
                };
                await _httpRequestSender.PostAsync(interactionDto.ResponseUrl, updatedMsg);
                await _notificationService.PublishPollVoteChanged(updatedPoll.Owner, updatedPoll.Id);
            }
            catch (System.Exception e)
            {
                bool isInternalError = !(e is CantMultiVoteException);
                string warningMessage = isInternalError ? 
                    "Unexpected error, try again later" : "You cannot vote for multiple options in this poll";
                var warningModalDto = new ShowViewDto
                {
                    TriggerId = interactionDto.TriggerId,
                    View = ModalUiHelper.BuildWarningModal(warningMessage)
                };

                await _httpRequestSender.PostToSlackByPathAsync(CommonSlackPaths.OpenViewPath, warningModalDto);
                
                if (isInternalError)
                {
                    throw;
                }
            }
        }
    }
}