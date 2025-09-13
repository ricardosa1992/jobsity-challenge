using Microsoft.AspNetCore.Identity;

namespace JobsityChallenge.Core.Entities;

public class ChatRoom
{
    public int Id { get; set; }
    public string Name { get; set; }
    public readonly List<ChatMessage> _messages;
    public virtual IReadOnlyCollection<ChatMessage> Messages => _messages.AsReadOnly();
    public readonly List<IdentityUser> _users;
    public virtual IReadOnlyCollection<IdentityUser> Users => _users.AsReadOnly();

    private ChatRoom()
    {
        _messages = [];
        _users = [];
    }

    public ChatRoom(string name) : this()
    {
        Name = name;
    }

    public void AddMessage(string content, string userId)
    {
        _messages.Add(new ChatMessage(this, content, userId));
    }
}
