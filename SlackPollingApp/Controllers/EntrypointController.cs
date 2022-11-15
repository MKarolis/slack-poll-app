using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SlackPollingApp.Business.Service;
using SlackPollingApp.Model.Slack.Data;
using SlackPollingApp.Model.Slack.Request;

namespace SlackPollingApp.Controllers
{
    [ApiController]
    [Route("api/entrypoint")]
    public class EntrypointController : ControllerBase
    {

        private readonly SlashCommandService _slashCommandService;
        private readonly ActionService _actionService;
        
        public EntrypointController(SlashCommandService slashCommandService, ActionService actionService)
        {
            _slashCommandService = slashCommandService;
            _actionService = actionService;
        }

        [HttpPost("pollo")]
        public async Task SlashCommandEntrypoint([FromForm] SlashCommandInvokedRequest request)
        {
            await _slashCommandService.HandleSlashCommandEntry(request);
        }

        [HttpPost("action")]
        public async Task ActionEntrypoint([FromForm] Dictionary<string, string> request)
        {
            var payload = JsonSerializer.Deserialize<SlackInteractionDto>(request["payload"]);
            await _actionService.HandleIncomingAction(payload);
        }
    }
}