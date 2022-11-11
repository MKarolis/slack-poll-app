using System.Threading.Tasks;
using SlackPollingApp.Business.Helper;
using SlackPollingApp.Core.Http;
using SlackPollingApp.Model.Slack.Helper;
using SlackPollingApp.Model.Slack.Request;

using static SlackPollingApp.Model.Slack.Common.CommonSlackPaths;

namespace SlackPollingApp.Business.Service
{
    public class SlashCommandService
    {
        private readonly HttpRequestSender _httpRequestSender;

        public SlashCommandService(HttpRequestSender httpRequestSender)
        {
            _httpRequestSender = httpRequestSender;
        }
        
        public async Task HandleSlashCommandEntry (SlashCommandInvokedRequest request)
        {
            var modal = ModalUiHelper.BuildInitialModal();
            modal.PrivateMetadata = MetadataHelper.FormatModalMetadata(request.ChannelId);
            var showModalDto = new ShowViewDto
            {
                TriggerId = request.TriggerId,
                View = modal
            };
            
            await _httpRequestSender.PostToSlackByPathAsync(OpenViewPath, showModalDto);
        }
    }
}