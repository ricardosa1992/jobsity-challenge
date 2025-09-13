namespace JobsityChallenge.Shared.Hubs;

public interface ISignalRService
{
    Task SendMessage(string chatRoomId, string user, string message);
}