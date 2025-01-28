namespace VibeoServer.Models
{
    public class History
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int VideoId { get; set; }
        public Video Video { get; set; }
        public DateTime WatchedAt { get; set; }
    }
}
