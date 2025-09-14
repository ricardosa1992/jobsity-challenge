using JobsityChallenge.Core.DTOs;
using JobsityChallenge.Core.Entities;
using JobsityChallenge.Core.Interfaces.Repositories;
using JobsityChallenge.Core.Parameters;
using JobsityChallenge.Core.Results;
using JobsityChallenge.Core.Services;
using JobsityChallenge.Shared.Hubs;
using JobsityChallenge.Shared.MessageBroker.Events;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace JobsityChallenge.Tests.Services;

public class ChatServiceTests
{
    private readonly Mock<IChatRepository> _chatRepositoryMock;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly Mock<IHubContext<ChatHub>> _hubContextMock;
    private readonly Mock<IClientProxy> _clientProxyMock;
    private readonly Mock<IHubClients> _hubClientsMock;
    private readonly Mock<IGroupManager> _groupManagerMock;

    private readonly ChatService _chatService;

    public ChatServiceTests()
    {
        _chatRepositoryMock = new Mock<IChatRepository>();

        var store = new Mock<IUserStore<IdentityUser>>();
        _userManagerMock = new Mock<UserManager<IdentityUser>>(
            store.Object, null, null, null, null, null, null, null, null
        );

        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _hubContextMock = new Mock<IHubContext<ChatHub>>();
        _hubClientsMock = new Mock<IHubClients>();
        _clientProxyMock = new Mock<IClientProxy>();
        _groupManagerMock = new Mock<IGroupManager>();

        _hubContextMock.Setup(h => h.Clients).Returns(_hubClientsMock.Object);
        _hubClientsMock.Setup(c => c.Group(It.IsAny<string>())).Returns(_clientProxyMock.Object);

        _chatService = new ChatService(
            _chatRepositoryMock.Object,
            _userManagerMock.Object,
            _publishEndpointMock.Object,
            _hubContextMock.Object
        );
    }

    [Fact]
    public async Task GetChatRoomsAsync_ShouldReturnPagedResult()
    {
        // Arrange
        var parameters = new GetChatRoomsParameters { PageSize = 2 };
        var rooms = new List<ChatRoom> { new("Room1"), new("Room2") };
        var pagedResult = new PagedResult<ChatRoom>
        {
            Items = rooms,
            TotalCount = 2
        };

        _chatRepositoryMock.Setup(r => r.GetChatRoomsAsync(parameters))
                           .ReturnsAsync(pagedResult);

        // Act
        var result = await _chatService.GetChatRoomsAsync(parameters);

        // Assert
        Assert.Equal(2, result.Items.Count());
        Assert.Equal(2, result.TotalCount);
    }

    [Fact]
    public async Task CreateChatRoomAsync_ShouldReturnCreatedRoom()
    {
        // Arrange
        var dto = new CreateChatRoomDto("RoomX");
        var createdRoom = new ChatRoom("RoomX") { Id = 1 };

        _chatRepositoryMock.Setup(r => r.CreateChatRoomAsync(It.IsAny<ChatRoom>()))
                           .ReturnsAsync(createdRoom);

        // Act
        var result = await _chatService.CreateChatRoomAsync(dto);

        // Assert
        Assert.Equal(1, result.Id);
        Assert.Equal("RoomX", result.Name);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldReturnError_WhenUserNotFound()
    {
        // Arrange
        var dto = new SendMessageDto { UserId = "u1", RoomId = 1, Content = "Hello" };
        _userManagerMock.Setup(u => u.FindByIdAsync(dto.UserId))
                        .ReturnsAsync((IdentityUser)null!);

        // Act
        var result = await _chatService.SendMessageAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Errors.UserNotFound, result);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldReturnError_WhenChatRoomNotFound()
    {
        // Arrange
        var dto = new SendMessageDto { UserId = "u1", RoomId = 1, Content = "Hello" };
        var user = new IdentityUser { Id = "u1", UserName = "John" };

        _userManagerMock.Setup(u => u.FindByIdAsync(dto.UserId)).ReturnsAsync(user);
        _chatRepositoryMock.Setup(r => r.GetChatRoomByIdAsync(dto.RoomId))
                           .ReturnsAsync((ChatRoom)null!);

        // Act
        var result = await _chatService.SendMessageAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Errors.ChatRoomNotFound, result);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldPublishStockEvent_WhenMessageIsStockCommand()
    {
        // Arrange
        var dto = new SendMessageDto { UserId = "u1", RoomId = 1, Content = "/stock=aapl.us" };
        var user = new IdentityUser { Id = "u1", UserName = "John" };
        var room = new ChatRoom("Room1") { Id = 1 };

        _userManagerMock.Setup(u => u.FindByIdAsync(dto.UserId)).ReturnsAsync(user);
        _chatRepositoryMock.Setup(r => r.GetChatRoomByIdAsync(dto.RoomId)).ReturnsAsync(room);

        // Act
        var result = await _chatService.SendMessageAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);
        _publishEndpointMock.Verify(p => p.Publish(It.IsAny<StockQuoteBotEvent>(), default), Times.Once);
        _clientProxyMock.Verify(c => c.SendCoreAsync("ReceiveMessage",
            It.Is<object[]>(o => (string)o[2] == dto.Content), default), Times.Once);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldSaveMessage_WhenNormalMessage()
    {
        // Arrange
        var dto = new SendMessageDto { UserId = "u1", RoomId = 1, Content = "Hello World" };
        var user = new IdentityUser { Id = "u1", UserName = "John" };
        var room = new ChatRoom("Room1") { Id = 1 };

        _userManagerMock.Setup(u => u.FindByIdAsync(dto.UserId)).ReturnsAsync(user);
        _chatRepositoryMock.Setup(r => r.GetChatRoomByIdAsync(dto.RoomId)).ReturnsAsync(room);

        // Act
        var result = await _chatService.SendMessageAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);
        _chatRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        _clientProxyMock.Verify(c => c.SendCoreAsync("ReceiveMessage",
            It.Is<object[]>(o => (string)o[2] == dto.Content), default), Times.Once);
    }

    [Fact]
    public async Task GetMessagesAsync_ShouldReturnMessages()
    {
        // Arrange
        var messages = new List<GetMessageDto>
        {
            new(1, "123", "test", "Hello", DateTime.Now)
        };
        _chatRepositoryMock.Setup(r => r.GetLastMessagesAsync(1, 50))
                           .ReturnsAsync(messages);

        // Act
        var result = await _chatService.GetMessagesAsync(1);

        // Assert
        Assert.Single(result);
        Assert.Equal("Hello", result.First().Content);
    }
}
