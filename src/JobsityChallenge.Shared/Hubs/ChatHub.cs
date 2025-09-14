using Microsoft.AspNetCore.SignalR;

namespace JobsityChallenge.Shared.Hubs;

public class ChatHub : Hub
{
    public async Task JoinRoom(string chatRoomId)
      => await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);

    public async Task LeaveRoom(string chatRoomId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);

    public async Task SendMessage(string chatRoomId, string user, string message)
    {
        await Clients.Group(chatRoomId).SendAsync("ReceiveMessage", chatRoomId,  user, message);
    }
}
