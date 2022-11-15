using MongoDB.Driver;
using SlackPollingApp.Model.Entity;

namespace SlackPollingApp.Tests.Config;

public class BaseIT
{
    private readonly CustomWebApplicationFactory _webApplicationFactory;
    protected ExternalServicesMock ExternalServicesMock { get; }
    
    protected readonly IMongoCollection<Poll> PollCollection;

    protected BaseIT()
    {
        ExternalServicesMock = new ExternalServicesMock();
        _webApplicationFactory = new CustomWebApplicationFactory(ExternalServicesMock);

        var dbFixture = new DockerMongoDbFixture();
        PollCollection = dbFixture.PollCollection;
    }

    protected HttpClient GetClient() => _webApplicationFactory.CreateClient();
}