using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SlackPollingApp.Hubs;

namespace SlackPollingApp.Business.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<PollHub> _hub;

        public NotificationService(IHubContext<PollHub> hub)
        {
            _hub = hub;
        }

        public async Task PublishPollCreated(string owner)
        {
            await _hub.Clients.All.SendAsync(PollHub.EventPollCreated, owner);
        }
        
        public async Task PublishPollVoteChanged(string owner, string pollId)
        {
            await _hub.Clients.All.SendAsync(PollHub.EventPollVoteChanged, owner, pollId);
        }
    }
}