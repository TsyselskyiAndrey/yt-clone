using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VibeoServer.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? EmailConfirmationCode { get; set; }
        public DateTime EmailConfirmationCodeExpiryTime { get; set; }
        public Channel? Channel { get; set; }
        public List<VideoStatus> VideoStatuses { get; set; } = new List<VideoStatus>();
        public List<History> Histories { get; set; } = new List<History>();
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public List<Playlist> Playlists { get; set; } = new List<Playlist>();
        public List<CommentStatus> CommentStatuses { get; set; } = new List<CommentStatus>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
