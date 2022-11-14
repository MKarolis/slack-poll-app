using System.Threading.Tasks;
using MongoDB.Driver;
using SlackPollingApp.Core.Config;
using SlackPollingApp.Model.Entity;

namespace SlackPollingApp.Model.Repository
{
    public class PollUpdateRepositoryImpl : IPollUpdateRepository
    {
        private readonly IMongoCollection<Poll> _polls;

        public PollUpdateRepositoryImpl(MongoContext mongoContext)
        {
            _polls = mongoContext.Database.GetCollection<Poll>("Polls");
            _polls.Indexes.CreateOne(new CreateIndexModel<Poll>("{ MessageTimestamp: 1 }"));
        }

        public async Task Insert(Poll poll)
        {
            await _polls.InsertOneAsync(poll);
        }

        public async Task AddVote(string pollId, string optionValue, UserSummary userSummary)
        {
            var filter = Builders<Poll>.Filter;
            var pollAndOptionFilter = filter.And(
                filter.Eq(poll => poll.Id, pollId),
                filter.Eq("Options.Value", optionValue));
            
            var update = Builders<Poll>.Update.AddToSet("Options.$.Votes", userSummary);
            await _polls.UpdateOneAsync(pollAndOptionFilter, update);
        }
        
        public async Task RemoveVote(string pollId, string optionValue, UserSummary userSummary)
        {
            var filter = Builders<Poll>.Filter;
            var pollAndOptionFilter = filter.And(
                filter.Eq(poll => poll.Id, pollId),
                filter.Eq("Options.Value", optionValue));
            
            var update = Builders<Poll>.Update.Pull("Options.$.Votes", userSummary);
            await _polls.UpdateOneAsync(pollAndOptionFilter, update);
        }

        public async Task UpdatePollTimestamp(string pollId, string ts)
        {
            var filter = Builders<Poll>.Filter.Eq(p => p.Id, pollId);
            var update = Builders<Poll>.Update.Set(p => p.MessageTimestamp, ts);
            await _polls.UpdateOneAsync(filter, update);
        }

        public async Task SetPollLocked(string pollId, bool locked)
        {
            var filter = Builders<Poll>.Filter.Eq(p => p.Id, pollId);
            var update = Builders<Poll>.Update.Set(p => p.Locked, locked);
            await _polls.UpdateOneAsync(filter, update);
        }

        public async Task DeleteById(string pollId)
        {
            await _polls.DeleteOneAsync(Builders<Poll>.Filter.Eq(p => p.Id, pollId));
        }
    }
}