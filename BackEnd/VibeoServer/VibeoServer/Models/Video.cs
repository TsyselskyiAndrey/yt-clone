namespace VibeoServer.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Url { get; set; }
        public string Format { get; set; }
        public long Size { get; set; }
        public string? PreviewUrl { get; set; }
        public string? Description { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsPublic { get; set; }
        public bool IsUploaded { get; set; }
        public int ChannelId {get; set;}
        public Channel Channel {get; set;}
        public List<VideoStatus> VideoStatuses { get; set; } = new List<VideoStatus>();
        public List<History> Histories { get; set; } = new List<History>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<Playlist> Playlists { get; set; } = new List<Playlist>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
