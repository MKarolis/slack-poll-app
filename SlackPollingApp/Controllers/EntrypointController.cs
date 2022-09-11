using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SlackPollingApp.Business.Service;
using SlackPollingApp.Hubs;
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
        public async void SlashCommandEntrypoint([FromForm] SlashCommandInvokedRequest request)
        {
            await _slashCommandService.HandleSlashCommandEntry(request);
        }

        [HttpPost("action")]
        public async void ActionEntrypoint([FromForm] Dictionary<string, string> request)
        {
            var payload = JsonSerializer.Deserialize<SlackInteractionDto>(request["payload"]);
            await _actionService.HandleIncomingAction(payload);
        }
    }
}