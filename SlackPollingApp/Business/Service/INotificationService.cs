using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SlackPollingApp.Hubs;

namespace SlackPollingApp.Business.Service
{
    public interface INotificationService
    {

        Task PublishPollCreated(string owner);

        Task PublishPollVoteChanged(string owner, string pollId);
    }
}