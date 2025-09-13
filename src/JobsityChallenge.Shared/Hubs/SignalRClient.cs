using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace JobsityChallenge.Shared.Hubs;

public class SignalRClient : ISignalRService
{
    private readonly HubConnection _connection;

    public SignalRClient(string? url)
    {
        try
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect([
                    TimeSpan.FromMilliseconds(1),
                    TimeSpan.FromMilliseconds(200),
                    TimeSpan.FromSeconds(1)
                ])
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Error);
                })
                .Build();

            _connection.Closed += async (error) =>
            {
                if (error != null) Console.WriteLine($"SignalR Status: {error}", nameof(SignalRClient));
                await Task.Delay(new Random().Next(0, 2) * 1000);
                await _connection.StartAsync();
            };

            TryToStart();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{nameof(SignalRClient)}. {ex.Message}");
        }
    }

    private void TryToStart()
    {
        try
        {
            if (_connection.State == HubConnectionState.Disconnected)
                _connection.StartAsync().Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error when Start SignalR connection | Error : {ex.Message}");
        }
    }

    public async Task SendMessage(string chatRoomId, string user, string message)
    {
        try
        {
            TryToStart();
            await _connection.SendAsync("SendMessage", chatRoomId, user, message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error at SignalRClient {nameof(SendMessage)} | Error : {ex.Message}");
        }
    }
}
