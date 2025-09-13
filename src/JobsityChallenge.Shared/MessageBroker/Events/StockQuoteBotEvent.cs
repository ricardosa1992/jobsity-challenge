namespace JobsityChallenge.Shared.MessageBroker.Events;

public record StockQuoteBotEvent(string? UserId, int RoomId, string Content);
