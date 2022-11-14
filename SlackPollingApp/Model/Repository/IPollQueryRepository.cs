using System.Collections.Generic;
using System.Threading.Tasks;
using SlackPollingApp.Model.Entity;

namespace SlackPollingApp.Model.Repository
{
    public interface IPollQueryRepository
    {
        Task<Poll> GetPollByTs(string ts);
        Task<Poll> GetPollById(string id);
        Task<List<Poll>> GetUserPolls(string userId);
    }
}