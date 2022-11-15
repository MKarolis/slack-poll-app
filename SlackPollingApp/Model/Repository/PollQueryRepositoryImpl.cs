using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using SlackPollingApp.Core.Config;
using SlackPollingApp.Model.Entity;

namespace SlackPollingApp.Model.Repository
{
    public class PollQueryRepositoryImpl : IPollQueryRepository
    {
        private readonly IMongoCollection<Poll> _polls;
        
        public PollQueryRepositoryImpl(MongoContext mongoContext)
        {
            _polls = mongoContext.Database.GetCollection<Poll>("Polls");
        }

        public async Task<Poll> GetPollByTs(string ts)
        {
            var res = await _polls.FindAsync(Builders<Poll>.Filter.Eq(p => p.MessageTimestamp, ts));
            return res.SingleOrDefault();
        }
        
        public async Task<Poll> GetPollById(string id)
        {
            var res = await _polls.FindAsync(Builders<Poll>.Filter.Eq(p => p.Id, id));
            return res.SingleOrDefault();
        }

        public async Task<List<Poll>> GetUserPolls(string userId)
        {
            var res = await _polls.FindAsync(
                Builders<Poll>.Filter.Eq(p => p.Owner, userId),
                new FindOptions<Poll>()
                {
                    Sort = Builders<Poll>.Sort.Descending(p => p.Date)
                }
            );
            return await res.ToListAsync();
        }
    }
}