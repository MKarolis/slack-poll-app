using System.Text.Json;
using MongoDB.Driver;
using Moq;
using SlackPollingApp.Model.Entity;
using SlackPollingApp.Model.Slack.Data;
using SlackPollingApp.Model.Slack.Object;
using SlackPollingApp.Model.Slack.Request;
using static SlackPollingApp.Model.Slack.Common.ActionConstants;
using static SlackPollingApp.Model.Slack.Common.BlockBuildingConstants;
using static SlackPollingApp.Model.Slack.Common.CommonSlackPaths;
using Option = SlackPollingApp.Model.Slack.Object.Option;

namespace SlackPollingApp.Tests.ControllerTests.ActionControllerTests;

[TestFixture]
public class ModalSubmissionIT : ActionsBaseIT
{
    private const string ViewId = "VIEW_ID";
    private const string ChannelId = "CHANNEL_ID";
    private const string UserId = "USER_ID";
    private const string Title = "TITLE";
    private static readonly string Timestamp = Guid.NewGuid().ToString();

    [Test]
    public async Task ActionEntrypoint_ModalSubmission_ShouldCreateThePoll()
    {
        ExternalServicesMock.HttpRequestSender.Setup(
                s => s.PostToSlackByPathAsync(PostMessagePath, It.IsAny<PostMessageDto>())
            ).ReturnsAsync(BuildMessageCreatedResponse()
        );
        await Dispatch(BuildPayload(ActionIdAddOption, 2));

        var apiCallArgs = ExternalServicesMock.HttpRequestSender.Invocations[0].Arguments;
        
        string path = (string)apiCallArgs[0];
        PostMessageDto postMessageDto = (PostMessageDto)apiCallArgs[1];
        
        Assert.That(path, Is.EqualTo(PostMessagePath));
        
        AssertMessagePosted(postMessageDto);
        await AssertPollCreated();
    }

    private void AssertMessagePosted(PostMessageDto postMessageDto)
    {
        Assert.That(postMessageDto.Channel, Is.EqualTo(ChannelId));
        var blocks = postMessageDto.Blocks;
        
        Assert.That(blocks[0].Text.Text, Does.Contain(Title));

        var sectionBlocks = blocks.FindAll(b => b.Type == BlockTypeSection);
        
        Assert.That(sectionBlocks.Count, Is.EqualTo(3));
        Assert.That(sectionBlocks[1].Text.Text, Does.Contain("OPTION"));
        Assert.That(sectionBlocks[1].Accessory, Is.Not.Null);
        Assert.That(sectionBlocks[2].Text.Text, Does.Contain("OPTION"));
        Assert.That(sectionBlocks[2].Accessory, Is.Not.Null);
    }

    private async Task AssertPollCreated()
    {
        var queryResult = await PollCollection.FindAsync(
            Builders<Poll>.Filter.Eq(p => p.MessageTimestamp, Timestamp)
        );
        Poll poll = queryResult.SingleOrDefault();
        
        Assert.That(poll.MessageTimestamp, Is.EqualTo(Timestamp));
        Assert.That(poll.Owner, Is.EqualTo(UserId));
        Assert.That(poll.Title, Is.EqualTo(Title));

        var options = poll.Options;
        Assert.That(options[0].Value, Does.Contain("OPTION"));
        Assert.That(options[1].Value, Does.Contain("OPTION"));
    }
    
    private SlackInteractionDto BuildPayload(String actionId, int optionCount)
    {
        var blocks = new List<Block>();
        var values = new Dictionary<string, FieldStateDto>();
        for (var i = 0; i < optionCount; i++)
        {
            var blockActionId = "OPTION-" + (i + 1);
            blocks.Add(new Block
            {
                Type = BlockTypeInput,
                Element = new Element
                {
                    ActionId = blockActionId
                }
            });

            values[blockActionId] = new FieldStateDto()
            {
                Value = blockActionId
            };
        }

        values[ActionIdQuestion] = new FieldStateDto()
        {
            Value = Title
        };
        values[ActionIdCheckboxes] = new FieldStateDto()
        {
            SelectedOptions = new List<Option>()
        };

        return new SlackInteractionDto
        {
            Type = ActionTypeViewSubmission,
            Container = new SlackContainerDto
            {
                Type = ContainerTypeView,
                ViewId = ViewId,
            },
            View = new View
            {
                Blocks = blocks,
                State = new ViewState
                {
                    Values = new Dictionary<string, Dictionary<string, FieldStateDto>>()
                    {
                        { ViewId, values }
                    }
                },
                PrivateMetadata = "channelId=" + ChannelId
            },
            User = new SlackUserDto
            {
                Id = UserId
            },
            Actions = new List<SlackActionDto>
            {
                new()
                {
                    ActionId = actionId
                }
            }
        };
    }

    private String BuildMessageCreatedResponse()
    {
        var message = new MessageCreatedDto
        {
            Ok = true,
            Ts = Timestamp
        };
        return JsonSerializer.Serialize(message);
    }
}