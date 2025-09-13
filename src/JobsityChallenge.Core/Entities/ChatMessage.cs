using Microsoft.AspNetCore.Identity;

namespace JobsityChallenge.Core.Entities;

public class ChatMessage
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; }
    public virtual IdentityUser User { get; set; }
    public int ChatRoomId { get; set; }
    public virtual ChatRoom ChatRoom { get; set; }

    private ChatMessage() { }
}
