namespace JobsityChallenge.Client.Models
{
    public class Message
    {
        public string? UserId { get; set; }
        public int RoomId { get; set; }
        public string Content { get; set; }
        public int MessageId { get; set; }
        public string UserName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
