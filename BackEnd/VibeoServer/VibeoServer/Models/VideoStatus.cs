namespace VibeoServer.Models
{
    public class VideoStatus
    {
        public int Id { get; set; }
        public bool? IsLiked { get; set; }
        public bool IsFavorite { get; set; }
        public bool WatchLater { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int VideoId { get; set; }
        public Video Video { get; set; }

    }
}
