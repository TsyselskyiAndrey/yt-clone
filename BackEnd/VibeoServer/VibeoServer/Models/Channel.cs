namespace VibeoServer.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LogoUrl { get; set; }
        public string Handle { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public List<Video> Videos { get; set; } = new List<Video>();
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
