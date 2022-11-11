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
    public class PollService : IPollService
    {
        private readonly PollRepository _pollRepository;
        private readonly HttpRequestSender _httpRequestSender;

        public PollService(PollRepository pollRepository, HttpRequestSender httpRequestSender)
        {
            _pollRepository = pollRepository;
            _httpRequestSender = httpRequestSender;
        }

        public async Task<Poll> CreatePoll(SlackInteractionDto interactionDto)
        {
            var poll = PollMapper.map(interactionDto);
            await _pollRepository.Insert(poll);

            return poll;
        }

        public async Task<Poll> ToggleVote(SlackInteractionDto interactionDto)
        {
            var messageTs = interactionDto.Container.MessageTs;
            var option = interactionDto.Actions.First().ActionId;
            var userId = interactionDto.User.Id;
            
            var poll = await _pollRepository.GetPollByTs(messageTs);
            ValidateCanVote(poll, userId, option);
            
            if (poll.Options.Find(o => o.Value == option)!.Votes.Any(v => v.Id == userId))
            {
                await _pollRepository.RemoveVote(poll.Id, option, new UserSummary{Id = userId, Name = interactionDto.User.Name});
            }
            else
            {
                await _pollRepository.AddVote(poll.Id, option, new UserSummary{Id = userId, Name = interactionDto.User.Name} );
            }
            
            return await _pollRepository.GetPollByTs(messageTs);
        }

        private void ValidateCanVote(Poll poll, string userId, string option)
        {
            if (poll.MultiSelect) return;
            if (poll.Options.Any(o => o.Value != option && o.Votes.Any(v => v.Id == userId)))
            {
                throw new CantMultiVoteException();
            }
        }

        public async Task UpdatePollTimestamp(string pollId, string ts)
        {
            await _pollRepository.UpdatePollTimestamp(pollId, ts);
        }

        public async Task SetPollLocked(string pollId, bool locked)
        {
            await _pollRepository.SetPollLocked(pollId, locked);
            var updatedPoll = await _pollRepository.GetPollById(pollId);
            var updatedMsg = new PostMessageDto
            {
                Channel = updatedPoll.ChannelId,
                Ts = updatedPoll.MessageTimestamp,
                Blocks = MessageUiHelper.RebuildUpdatedMessageBlocks(updatedPoll)
            };
            await _httpRequestSender.PostToSlackAsync(CommonSlackPaths.EditMessagePath, updatedMsg);
        }

        public async Task<List<Poll>> GetUserPollsAsync(string userId)
        {
            return await _pollRepository.GetUserPolls(userId);
        }

        public async Task DeletePollById(string pollId)
        {
            var poll = await _pollRepository.GetPollById(pollId);
            await _pollRepository.DeleteById(pollId);

            var deleteMessageDto = new DeleteMessageDto
            {
                Channel = poll.ChannelId,
                Ts = poll.MessageTimestamp
            };

            await _httpRequestSender.PostToSlackAsync(CommonSlackPaths.DeleteMessagePath, deleteMessageDto);
        }

        public async Task<Poll> GetPollByIdAsync(string pollId)
        {
            return await _pollRepository.GetPollById(pollId);
        }
    }
}