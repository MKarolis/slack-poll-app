using System.Text.Json;
using Moq;
using SlackPollingApp.Model.Slack.Data;
using SlackPollingApp.Tests.Config;

namespace SlackPollingApp.Tests.ControllerTests.ActionControllerTests;

public class ActionsBaseIT : BaseIT
{
    private HttpClient _httpClient;

    [SetUp]
    public void SetUp()
    {
        _httpClient = GetClient();
    }

    [TearDown]
    public void TearDown()
    {
        ExternalServicesMock.HttpRequestSender.Reset();
    }
    
    protected async Task Dispatch(SlackInteractionDto interactionDto)
    {
        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("payload", JsonSerializer.Serialize(interactionDto)), 
        });
        
        await _httpClient.PostAsync("api/entrypoint/action", formContent);
    }
}