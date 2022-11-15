using SlackPollingApp.Tests.Config;

namespace SlackPollingApp.Tests.ControllerTests;

[SetUpFixture]
public class SetupFixture : BaseIT
{
    [OneTimeTearDown]
    public void TearDown()
    {
        PollCollection.DeleteMany("{}");
    }
}