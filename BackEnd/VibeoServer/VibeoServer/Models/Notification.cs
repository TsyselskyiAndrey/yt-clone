namespace VibeoServer.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int NotificationTypeId { get; set; }
        public NotificationType NotificationType { get; set; }
        public int? VideoId { get; set; }
        public Video? Video { get; set; }
        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }
    }
}
