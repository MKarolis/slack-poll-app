using System.Text.Json;
using System.Threading.Tasks;
using SlackPollingApp.Business.Exception;
using SlackPollingApp.Core.Http;
using SlackPollingApp.Model.Slack.Data;
using SlackPollingApp.Model.Slack.Helper;
using SlackPollingApp.Model.Slack.Request;
using static SlackPollingApp.Model.Slack.Common.ActionConstants;
using static SlackPollingApp.Model.Slack.Common.CommonSlackPaths;
using static SlackPollingApp.Model.Slack.Common.BlockBuildingConstants;

namespace SlackPollingApp.Business.Service
{
    public class ActionService
    {
        private readonly IHttpRequestSender _httpRequestSender;
        private readonly IPollService _pollService;
        private readonly INotificationService _notificationService;

        public ActionService(IHttpRequestSender httpRequestSender, IPollService pollService, INotificationService notificationService)
        {
            _httpRequestSender = httpRequestSender;
            _pollService = pollService;
            _notificationService = notificationService;
        }

        public async Task HandleIncomingAction(SlackInteractionDto interactionDto)
        {
            switch (interactionDto.Type)
            {
                case ActionTypeBlockActions:
                    await HandleBlockAction(interactionDto);
                    break;
                case ActionTypeViewSubmission:
                    await HandleModalSubmission(interactionDto);
                    break;
                default:
                    return;
            }
        }

        private async Task HandleBlockAction(SlackInteractionDto interactionDto)
        {
            switch (interactionDto.Container.Type)
            {
                case ContainerTypeView:
                    await HandleModalBlockAction(interactionDto);
                    break;
                case ContainerTypeMessage:
                    await HandleMessageBlockAction(interactionDto);
                    break;
                default:
                    return;
            }
        }

        private async Task HandleMessageBlockAction(SlackInteractionDto interactionDto)
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

                await _httpRequestSender.PostToSlackByPathAsync(OpenViewPath, warningModalDto);
                
                if (isInternalError)
                {
                    throw;
                }
            }
        }

        private async Task HandleModalBlockAction(SlackInteractionDto interactionDto)
        {
            var modal = interactionDto.View;
            modal.State = null;
            
            if (interactionDto.Actions.Exists(a => a.ActionId == ActionIdAddOption))
            {
                ModalUiHelper.AddQuestionOption(modal);
            }
            else if (interactionDto.Actions.Exists(a => a.ActionId == ActionIdRemoveOption))
            {
                ModalUiHelper.RemoveQuestionOption(modal);
            }

            var updateRequest = new UpdateViewDto
            {
                ViewId = interactionDto.Container.ViewId,
                View = modal
            };

            await _httpRequestSender.PostToSlackByPathAsync(UpdateViewPath, updateRequest);
        }
        
        private async Task HandleModalSubmission(SlackInteractionDto interactionDto)
        {
            var poll = await _pollService.CreatePoll(interactionDto);

            var messageDto = MessageUiHelper.ComposeInitialMessage(poll);
            var result = await _httpRequestSender.PostToSlackByPathAsync(PostMessagePath, messageDto);
            var resultDto = JsonSerializer.Deserialize<MessageCreatedDto>(result);

            await _pollService.UpdatePollTimestamp(poll.Id, resultDto?.Ts);
            await _notificationService.PublishPollCreated(poll.Owner);
        }
    }
}