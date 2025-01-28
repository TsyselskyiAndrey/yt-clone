namespace VibeoServer.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
        public DateTime SubscribedAt { get; set; }
        public bool ReceiveNotifications { get; set; }
    }
}
