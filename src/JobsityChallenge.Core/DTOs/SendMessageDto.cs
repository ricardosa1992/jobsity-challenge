using System.Text.Json.Serialization;

namespace JobsityChallenge.Core.DTOs;

public class SendMessageDto
{
    [JsonIgnore]
    public string? UserId { get; set; }
    [JsonIgnore]
    public int RoomId { get; set; }
    public string Content { get; set; }
}
