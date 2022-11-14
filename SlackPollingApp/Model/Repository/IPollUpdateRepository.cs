using System.Threading.Tasks;
using SlackPollingApp.Model.Entity;

namespace SlackPollingApp.Model.Repository
{
    public interface IPollUpdateRepository
    {
        Task Insert(Poll poll);
        Task AddVote(string pollId, string optionValue, UserSummary userSummary);
        Task RemoveVote(string pollId, string optionValue, UserSummary userSummary);
        Task UpdatePollTimestamp(string pollId, string ts);
        Task SetPollLocked(string pollId, bool locked);
        Task DeleteById(string pollId);
    }
}