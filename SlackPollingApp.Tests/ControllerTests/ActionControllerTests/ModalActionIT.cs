using MongoDB.Driver;
using Moq;
using SlackPollingApp.Model.Entity;
using SlackPollingApp.Model.Slack.Common;
using SlackPollingApp.Model.Slack.Data;
using SlackPollingApp.Model.Slack.Object;
using SlackPollingApp.Model.Slack.Request;
using static SlackPollingApp.Model.Slack.Common.BlockBuildingConstants;
using static SlackPollingApp.Model.Slack.Common.CommonSlackPaths;
using Option = SlackPollingApp.Model.Entity.Option;

namespace SlackPollingApp.Tests.ControllerTests.ActionControllerTests;

[TestFixture]
public class ModalActionIT : ActionsBaseIT
{
    private const string ViewId = "VIEW_ID";

    [Test]
    public async Task ActionEntrypoint_ModalAction_ShouldAddOption()
    {
        await Dispatch(BuildPayload(ActionIdAddOption, 2));
        
        var apiCallArgs = ExternalServicesMock.HttpRequestSender.Invocations[0].Arguments;
        
        string path = (string)apiCallArgs[0];
        UpdateViewDto updateViewDto = (UpdateViewDto)apiCallArgs[1];
        
        Assert.That(path, Is.EqualTo(UpdateViewPath));
        Assert.That(updateViewDto.ViewId, Is.EqualTo(ViewId));

        var blocks = updateViewDto.View.Blocks;
        Assert.That(blocks.FindAll(b => b.Type == BlockTypeInput).Count, Is.EqualTo(3));

        var actions = blocks.Find(b => b.Type == BlockTypeActions);
        Assert.That(actions!.Elements.Count, Is.EqualTo(2));
        
        Assert.That(actions.Elements[0].ActionId, Is.EqualTo(ActionIdRemoveOption));
        Assert.That(actions.Elements[1].ActionId, Is.EqualTo(ActionIdAddOption));
    }
    
    [Test]
    public async Task ActionEntrypoint_ModalAction_ShouldRemoveOption()
    {
        await Dispatch(BuildPayload(ActionIdRemoveOption, 3));
        
        var apiCallArgs = ExternalServicesMock.HttpRequestSender.Invocations[0].Arguments;
        
        string path = (string)apiCallArgs[0];
        UpdateViewDto updateViewDto = (UpdateViewDto)apiCallArgs[1];
        
        Assert.That(path, Is.EqualTo(UpdateViewPath));
        Assert.That(updateViewDto.ViewId, Is.EqualTo(ViewId));

        var blocks = updateViewDto.View.Blocks;
        Assert.That(blocks.FindAll(b => b.Type == BlockTypeInput).Count, Is.EqualTo(2));

        var actions = blocks.Find(b => b.Type == BlockTypeActions);
        Assert.That(actions!.Elements.Count, Is.EqualTo(1));
        
        Assert.That(actions.Elements[0].ActionId, Is.EqualTo(ActionIdAddOption));
    }
    
    private SlackInteractionDto BuildPayload(String actionId, int optionCount)
    {
        var blocks = new List<Block>();

        for (var i = 0; i < optionCount; i++)
        {
            blocks.Add(new Block
            {
                Type = BlockTypeInput,
                Element = new Element
                {
                    ActionId = "OPTION-" + (i + 1)
                }
            });
        }
        
        blocks.Add(new Block
        {
            Type = BlockTypeActions,
            Elements = new List<Element>()
        });
        
        
        return new SlackInteractionDto
        {
            Container = new SlackContainerDto
            {
                Type = ActionConstants.ContainerTypeView,
                ViewId = ViewId,
            },
            View = new View
            {
                Blocks = blocks
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

    // [Test]
    public async Task TestPing()
    {
        ExternalServicesMock.HttpRequestSender
            .Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync("LIOL");
        await PollCollection.InsertOneAsync(new Poll
        {
            ChannelId = "MetadataHelper.GetChannelId(interactionDto.View.PrivateMetadata)",
            Title = "fieldValuesById[ActionIdQuestion].Value",
            Owner = "interactionDto.User.Id",
            Options = new List<Option>(),
            Date = DateTime.Now,
            ShowVoters = false,
            MultiSelect = false
        });
        
        Console.WriteLine(PollCollection.Find(_ => true));
        var httpClient = GetClient();

        var response = await httpClient.GetStringAsync("api/entrypoint");
        Assert.That(response, Is.EqualTo("pong"));
    }
}