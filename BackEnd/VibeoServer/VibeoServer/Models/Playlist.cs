
namespace VibeoServer.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public List<Video> Videos { get; set; } = new List<Video>();
    }
}
