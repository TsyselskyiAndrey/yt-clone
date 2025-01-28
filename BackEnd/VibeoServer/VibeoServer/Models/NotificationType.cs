namespace VibeoServer.Models
{
    public class NotificationType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Notification> Notifications { get; set; } = new List<Notification>();

    }
}
