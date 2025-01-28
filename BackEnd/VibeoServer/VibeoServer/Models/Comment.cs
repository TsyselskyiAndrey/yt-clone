namespace VibeoServer.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
        public int VideoId { get; set; }
        public Video Video { get; set; }
        public int? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public List<Comment> ChildComments { get; set; } = new List<Comment>();
        public List<CommentStatus> CommentStatuses { get; set; } = new List<CommentStatus>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
