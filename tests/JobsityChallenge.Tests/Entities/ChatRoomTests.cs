using JobsityChallenge.Core.Entities;

namespace JobsityChallenge.Tests.Entities;

public class ChatRoomTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithName()
    {
        // Act
        var room = new ChatRoom("General");

        // Assert
        Assert.Equal("General", room.Name);
        Assert.Empty(room.Messages);
        Assert.Empty(room.Users);
    }

    [Fact]
    public void AddMessage_ShouldAddNewMessageToRoom()
    {
        // Arrange
        var room = new ChatRoom("General");

        // Act
        room.AddMessage("Hello World", "user1");

        // Assert
        Assert.Single(room.Messages);
        var message = room.Messages.First();
        Assert.Equal("Hello World", message.Content);
        Assert.Equal("user1", message.UserId);
        Assert.Equal(room, message.ChatRoom);
    }

    [Fact]
    public void UsersAndMessages_ShouldBeEmptyInitially()
    {
        // Arrange
        var room = new ChatRoom("General");

        // Assert
        Assert.Empty(room.Users);
        Assert.Empty(room.Messages);
    }
}
