using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SlackPollingApp.Model.Entity;

namespace SlackPollingApp.Tests.Config;

public class DockerMongoDbFixture : IDisposable
{
    private MongoClient _mongoDbClient { get; }
    
    private string _databaseName { get; }
    public IMongoCollection<Poll> PollCollection { get; }
    
    public DockerMongoDbFixture()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.IntegrationTesting.json")
            .Build();

        _databaseName = config.GetValue<string>("MongoConnection:DatabaseName");
        _mongoDbClient = new MongoClient(config.GetValue<string>("MongoConnection:ConnectionString"));
        
        var database = _mongoDbClient.GetDatabase(_databaseName);
        PollCollection = database.GetCollection<Poll>("Polls");
    }
 
    public void Dispose()
    {
    }
}