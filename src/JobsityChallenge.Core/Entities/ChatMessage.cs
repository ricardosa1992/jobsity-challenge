using Microsoft.AspNetCore.Identity;

namespace JobsityChallenge.Core.Entities;

public class ChatMessage
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; } 
    public string UserId { get; set; }
    public virtual IdentityUser User { get; set; }
    public int ChatRoomId { get; set; }
    public virtual ChatRoom ChatRoom { get; set; }

    private ChatMessage() { }

    public ChatMessage(
        ChatRoom chatRoom,
        string content, 
        string userId)
    {
        ChatRoom = chatRoom;
        Content = content;
        Timestamp = DateTime.UtcNow;
        UserId = userId;
    }
}
