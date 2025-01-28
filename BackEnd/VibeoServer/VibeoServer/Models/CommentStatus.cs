namespace VibeoServer.Models
{
    public class CommentStatus
    {
        public int Id { get; set; }
        public bool? isLiked { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
