namespace WhoBrokeTheBuild
{
    public class Settings
    {
        public string BuildsUrl { get; set; }
        public string BuildDetailedUrl { get; set; }
        public string[] BuildIds { get; set; }
        public User[] Users { get; set; }
    }

    public class User
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string[] Handles { get; set; }
    }
}