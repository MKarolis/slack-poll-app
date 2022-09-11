using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SlackPollingApp.Model.Entity
{
    public class Poll
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string ChannelId { get; set; }
        public string MessageTimestamp { get; set; }
        public DateTime Date { get; set; }
        public List<Option> Options { get; set; }
        public bool MultiSelect { get; set; }
        public bool ShowVoters { get; set; }
        public bool Locked { get; set; }
        public string Owner { get; set; }
    }
}