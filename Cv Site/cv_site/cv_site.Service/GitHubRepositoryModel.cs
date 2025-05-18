using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cv_site.Service
{
    public class GitHubRepositoryModel
    {
        public string Name { get; set; }
        public string HtmlUrl { get; set; }
        public string Language { get; set; }
        public int StargazersCount { get; set; }
        public int ForksCount { get; set; }
        public int OpenIssuesCount { get; set; }
        public DateTimeOffset? PushedAt { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string Owner { get; set; }
        public bool Archived { get; set; }
        public bool Private { get; set; }

    }
}
