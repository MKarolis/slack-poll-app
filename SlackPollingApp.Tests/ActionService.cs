using Moq;
using SlackPollingApp.Business.Exception;
using SlackPollingApp.Business.Service;
using SlackPollingApp.Core.Http;
using SlackPollingApp.Model.Entity;
using SlackPollingApp.Model.Slack.Common;
using SlackPollingApp.Model.Slack.Data;
using SlackPollingApp.Model.Slack.Request;

namespace SlackPollingApp.Tests;

public class ActionServiceTest
{
    
    private ActionService _actionService;
    
    private readonly Mock<IHttpRequestSender> _httpRequestSender = new();
    private readonly Mock<IPollService> _pollService = new();
    private readonly Mock<INotificationService> _notificationService = new();
    
    private readonly SlackInteractionDto _interactionDto = new()
    {
        ResponseUrl = "test.com",
        Type = ActionConstants.ActionTypeBlockActions,
        Container = new SlackContainerDto()
        {
            Type = ActionConstants.ContainerTypeMessage
        },
        TriggerId = "MockTrigger"
    };
    private readonly Poll _updatedPoll = new()
    {
        Title = "MockTitle",
        Owner = "MockOwner",
        Id = "MockId",
        Options = new List<Option>()
    };

    [SetUp]
    public void Setup()
    {
        _actionService = new ActionService(_httpRequestSender.Object, _pollService.Object, _notificationService.Object);
    }
    
    [TearDown]
    public void TearDown()
    {
        _httpRequestSender.Reset();
        _pollService.Reset();
        _notificationService.Reset();
    }

    [Test]
    public async Task HandleMessageBlockAction_HappyPath()
    {
        _pollService.Setup(s => s.ToggleVote(_interactionDto).Result).Returns(_updatedPoll);
        await _actionService.HandleIncomingAction(_interactionDto);
        
        _httpRequestSender.Verify(s => s.PostToSlackAsync(_interactionDto.ResponseUrl, 
            It.Is<PostMessageDto>(msg => msg.Blocks[0].Text.Text.Contains("MockTitle"))));
        _notificationService.Verify(
            s => s.PublishPollVoteChanged(_updatedPoll.Owner, _updatedPoll.Id));
    }
    
    [Test]
    public async Task HandleMessageBlockAction_CantVoteError()
    {
        _pollService.Setup(s => s.ToggleVote(_interactionDto).Result)
            .Throws(new CantMultiVoteException());
        await _actionService.HandleIncomingAction(_interactionDto);
            
        _httpRequestSender.Verify(
            s => s.PostToSlackAsync(It.IsAny<string>(), It.IsAny<PostMessageDto>()),
            Times.Never());
        _notificationService.Verify(
            s => s.PublishPollVoteChanged(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never());
        _httpRequestSender.Verify(
            s => s.PostToSlackAsync(
                It.IsAny<string>(), 
                It.Is<ShowViewDto>(view => view.View.Blocks[0].Text.Text.Contains("You cannot vote")))
            );
    }
    
    [Test]
    public void HandleMessageBlockAction_InternalError()
    {
        _pollService.Setup(s => s.ToggleVote(_interactionDto).Result)
            .Throws(new Exception());
        Assert.ThrowsAsync<Exception>(() => _actionService.HandleIncomingAction(_interactionDto));
        
        _httpRequestSender.Verify(
            s => s.PostToSlackAsync(It.IsAny<string>(), It.IsAny<PostMessageDto>()),
            Times.Never());
        _notificationService.Verify(
            s => s.PublishPollVoteChanged(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never());
        _httpRequestSender.Verify(
            s => s.PostToSlackAsync(
                It.IsAny<string>(), 
                It.Is<ShowViewDto>(view => view.View.Blocks[0].Text.Text.Contains("Unexpected error")))
        );
    }
}