using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SlackPollingApp.Hubs
{
    public class PollHub : Hub
    {
        public const string HubUrl = "/polls";
        public const string EventPollCreated = "PollCreated";
        public const string EventPollVoteChanged = "PollVoteChanged";

        public async Task PollCreated(string owner)
        {
            await Clients.All.SendAsync(EventPollCreated, owner);
        }
        
        public async Task PollVoteChanged(string owner, string pollId)
        {
            await Clients.All.SendAsync(EventPollVoteChanged, owner, pollId);
        }
        
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }
    }
}