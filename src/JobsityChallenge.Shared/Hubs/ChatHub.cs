using Microsoft.AspNetCore.SignalR;

namespace JobsityChallenge.Shared.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string chatRoomId, string user, string message)
    {
        await Clients.Group(chatRoomId).SendAsync("ReceiveMessage", user, message);
    }
}
