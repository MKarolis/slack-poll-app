using System.Collections.Generic;

namespace SlackPollingApp.Model.Entity
{
    public class Option
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public List<UserSummary> Votes { get; set; }
    }
}