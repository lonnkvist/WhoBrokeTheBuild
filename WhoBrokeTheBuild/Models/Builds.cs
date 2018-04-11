using Newtonsoft.Json;

namespace WhoBrokeTheBuild.Models
{
    public class Builds
    {
        [JsonProperty("build")]
        public Build[] Results { get; set; }
    }
}