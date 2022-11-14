using System.Threading.Tasks;
using SlackPollingApp.Core.Http;
using SlackPollingApp.Model.Slack.Common;
using SlackPollingApp.Model.Slack.Data;
using SlackPollingApp.Model.Slack.Helper;
using SlackPollingApp.Model.Slack.Request;

namespace SlackPollingApp.Business.Service.ActionHandler
{
    public class ModalBlockActionHandler : IActionHandler
    {
        private readonly IHttpRequestSender _httpRequestSender;

        public ModalBlockActionHandler(IHttpRequestSender httpRequestSender)
        {
            _httpRequestSender = httpRequestSender;
        }
        
        public async Task Handle(SlackInteractionDto interactionDto)
        {
            var modal = interactionDto.View;
            modal.State = null;
            
            if (interactionDto.Actions.Exists(a => a.ActionId == BlockBuildingConstants.ActionIdAddOption))
            {
                ModalUiHelper.AddQuestionOption(modal);
            }
            else if (interactionDto.Actions.Exists(a => a.ActionId == BlockBuildingConstants.ActionIdRemoveOption))
            {
                ModalUiHelper.RemoveQuestionOption(modal);
            }

            var updateRequest = new UpdateViewDto
            {
                ViewId = interactionDto.Container.ViewId,
                View = modal
            };

            await _httpRequestSender.PostToSlackByPathAsync(CommonSlackPaths.UpdateViewPath, updateRequest);
        }
    }
}