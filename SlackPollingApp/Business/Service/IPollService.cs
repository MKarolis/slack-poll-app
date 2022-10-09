using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlackPollingApp.Business.Exception;
using SlackPollingApp.Core.Http;
using SlackPollingApp.Model.Entity;
using SlackPollingApp.Model.Mapper;
using SlackPollingApp.Model.Repository;
using SlackPollingApp.Model.Slack.Common;
using SlackPollingApp.Model.Slack.Data;
using SlackPollingApp.Model.Slack.Helper;
using SlackPollingApp.Model.Slack.Request;

namespace SlackPollingApp.Business.Service
{
    public interface IPollService
    {
        Task<Poll> CreatePoll(SlackInteractionDto interactionDto);

        Task<Poll> ToggleVote(SlackInteractionDto interactionDto);

        Task UpdatePollTimestamp(string pollId, string ts);

        Task SetPollLocked(string pollId, bool locked);

        Task<List<Poll>> GetUserPollsAsync(string userId);

        Task DeletePollById(string pollId);

        Task<Poll> GetPollByIdAsync(string pollId);
    }
}