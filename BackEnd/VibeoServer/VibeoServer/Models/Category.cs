namespace VibeoServer.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Video> Videos { get; set; } = new List<Video>();
    }
}
