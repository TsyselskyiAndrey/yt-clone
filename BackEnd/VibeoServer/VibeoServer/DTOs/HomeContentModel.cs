namespace VibeoServer.DTOs
{
    public class HomeContentModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PublicationDate { get; set; }
        public string PreviewUrl { get; set; } 
        public string Duration { get; set; } 
        public int? NumberOfViews { get; set; } 
        public string ChannelLogo { get; set; }
        public int? ChannelId { get; set; }
        public string ChannelTitle { get; set; }
        public int PlaylistId { get; set; }
        public string LastUpdated { get; set; }
        public int? VideoCount { get; set; }
        public bool IsPlaylist { get; set; }
    }
}
