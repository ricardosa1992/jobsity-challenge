namespace JobsityChallenge.Core.DTOs;

public record GetMessageDto(
    int MessageId, 
    string UserId, 
    string UserName,
    string Content,
    DateTime Timestamp);
