using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WhoBrokeTheBuild.Models;

namespace WhoBrokeTheBuild.Pages
{
    public class IndexModel : PageModel
    {
        private static readonly HttpClient Client = new HttpClient();
        private static Settings _settings;
        public Dictionary<string, User> Breakers { get; } = new Dictionary<string, User>();

        public IndexModel(IOptions<Settings> settings)
        {
            _settings = settings.Value;
        }


        public async Task OnGet()
        {
            Client.DefaultRequestHeaders.Add("accept", "application/json");

            foreach (var id in _settings.BuildIds)
            {
                string json = await Client.GetStringAsync(_settings.BuildsUrl + id);

                var builds = JsonConvert.DeserializeObject<Builds>(json);

                Dictionary<string, Build> grouped = builds.Results.GroupBy(b => b.BranchName)
                    .ToDictionary(x => x.Key, x => x.OrderByDescending(b => b.Id).First());

                var broken = grouped.Where(g => !g.Value.Success).ToArray();

                if (broken.Length == 0)
                    continue;

                foreach (var build in broken)
                {
                    string detailsJson = await Client.GetStringAsync(_settings.BuildDetailedUrl + build.Value.Id);

                    var details = JsonConvert.DeserializeObject<BuildDetailed>(detailsJson);
                    var change = details.LastChanges.Change.FirstOrDefault();

                    if (change != null)
                    {
                        User guilty = _settings.Users.FirstOrDefault(u => u.Handles?.Contains(change.Username) ?? false)
                                      ?? new User
                                      {
                                          Name = change.Username,
                                          Image = "/images/clown.jpg"
                                      };
                        if (String.IsNullOrEmpty(guilty.Image))
                            guilty.Image = "/images/clown.jpg";

                        Breakers.Add(build.Key, guilty);
                        
                        Breakers.Add(build.Key+"0", _settings.Users.First());
                        Breakers.Add(build.Key+"1", _settings.Users.Skip(1).First());
                        Breakers.Add(build.Key+"2", _settings.Users.Skip(2).First());
                        Breakers.Add(build.Key+"3", _settings.Users.Skip(3).First());
                    }
                }
            }
        }
    }
}
