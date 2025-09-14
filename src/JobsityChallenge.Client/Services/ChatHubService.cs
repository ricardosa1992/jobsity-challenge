using Microsoft.AspNetCore.SignalR.Client;

public class ChatHubService : IAsyncDisposable
{
    private readonly HubConnection _hubConnection;

    public event Action<string, string, string>? OnMessageReceived;

    public ChatHubService()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5001/chathub") // Use your API container URL
             .ConfigureLogging(logging =>
             {    
                 logging.SetMinimumLevel(LogLevel.Debug); 
             })
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<string, string, string>("ReceiveMessage", (chatRoomId, user, message) =>
        {
            OnMessageReceived?.Invoke(chatRoomId, user, message);
        });
    }

    public async Task StartAsync()
    {
        if (_hubConnection.State == HubConnectionState.Disconnected)
            await _hubConnection.StartAsync();
    }

    public async Task InvokeAsync(string method, string chatRoomId)
    {
        if (_hubConnection.State == HubConnectionState.Connected)
            await _hubConnection.InvokeAsync(method, chatRoomId);
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }
}
