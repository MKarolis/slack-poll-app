using MongoDB.Driver;
using SlackPollingApp.Model.Entity;
using SlackPollingApp.Model.Slack.Data;
using SlackPollingApp.Model.Slack.Request;
using static SlackPollingApp.Model.Slack.Common.ActionConstants;
using static SlackPollingApp.Model.Slack.Common.BlockBuildingConstants;

namespace SlackPollingApp.Tests.ControllerTests.ActionControllerTests;

[TestFixture]
public class VotingActionIT : ActionsBaseIT
{
    private const string ChannelId = "CHANNEL_ID";
    private const string UserId = "USER_ID";
    private const string Title = "TITLE";
    private const string ResponseUrl = "response_url.com";
    private static readonly string Timestamp = Guid.NewGuid().ToString();

    [SetUp]
    public new void SetUp()
    {
        PollCollection.InsertOne(new Poll
        {
            ChannelId = ChannelId,
            Title = Title,
            Owner = UserId,
            Options = new List<Option>
            {
                new()
                {
                    Title = "OPTION-1",
                    Value = "OPTION-1",
                    Votes = new List<UserSummary>(),
                },
                new()
                {
                    Title = "OPTION-2",
                    Value = "OPTION-2",
                    Votes = new List<UserSummary>(),
                }
            },
            Date = DateTime.Now,
            MessageTimestamp = Timestamp,
            ShowVoters = false,
            MultiSelect = false
        });
    }
    
    [Test]
    public async Task ActionEntrypoint_MessageVote_ShouldUpdatePollAndMessage()
    {
        await Dispatch(BuildPayload("OPTION-1"));
        
        var apiCallArgs = ExternalServicesMock.HttpRequestSender.Invocations[0].Arguments;
        
        string url = (string)apiCallArgs[0];
        PostMessageDto postMessageDto = (PostMessageDto)apiCallArgs[1];
        
        Assert.That(url, Is.EqualTo(ResponseUrl));
        AssertMessageUpdated(postMessageDto);
        await AssertPollUpdated();
    }

    private void AssertMessageUpdated(PostMessageDto updateMessageDto)
    {
        Assert.That(updateMessageDto.ReplaceOriginal, Is.True);
        var blocks = updateMessageDto.Blocks;
        
        Assert.That(blocks[0].Text.Text, Does.Contain(Title));

        var sectionBlocks = blocks.FindAll(b => b.Type == BlockTypeSection);
        
        Assert.That(sectionBlocks.Count, Is.EqualTo(3));
        Assert.That(sectionBlocks[1].Text.Text, Does.Contain("OPTION-1"));
        Assert.That(sectionBlocks[1].Text.Text, Does.Contain("100% (1/1)"));
        Assert.That(sectionBlocks[1].Accessory, Is.Not.Null);
    }
    
    private async Task AssertPollUpdated()
    {
        var queryResult = await PollCollection.FindAsync(
            Builders<Poll>.Filter.Eq(p => p.MessageTimestamp, Timestamp)
        );
        Poll poll = queryResult.SingleOrDefault();

        var options = poll.Options;
        Assert.That(options[0].Votes.Count, Is.EqualTo(1));
        Assert.That(options[0].Votes[0].Id, Is.EqualTo(UserId));
        Assert.That(options[1].Value, Does.Contain("OPTION"));
    }
    
    private SlackInteractionDto BuildPayload(String actionId)
    {
        return new SlackInteractionDto
        {
            Type = ActionTypeBlockActions,
            Container = new SlackContainerDto
            {
                Type = ContainerTypeMessage,
                MessageTs = Timestamp
            },
            User = new SlackUserDto
            {
                Id = UserId,
                Name = "Username"
            },
            Actions = new List<SlackActionDto>
            {
                new()
                {
                    ActionId = actionId
                }
            },
            ResponseUrl = ResponseUrl,
        };
    }
}