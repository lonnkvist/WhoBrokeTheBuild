namespace WhoBrokeTheBuild.Models
{
    public class Build
    {
        public int Id { get; set; }
        public string BuildTypeId { get; set; }
        public string Number { get; set; }
        public bool Success => Status == "SUCCESS";
        public string Status { get; set; }
        public string State { get; set; }
        public string BranchName { get; set; }
        public bool DefaultBranch { get; set; }
        public string Href { get; set; }
        public string WebUrl { get; set; }
    }
}