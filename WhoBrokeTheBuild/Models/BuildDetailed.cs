namespace WhoBrokeTheBuild.Models
{
    public class BuildDetailed : Build
    {
        public BuildType BuildType { get; set; }
        public LastChanges LastChanges { get; set; }
        public User Guilty { get; set; }
    }
}