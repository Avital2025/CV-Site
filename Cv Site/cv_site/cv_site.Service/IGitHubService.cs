using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cv_site.Service
{
    public interface IGitHubService
    {
        Task<List<GitHubRepositoryModel>> GetPortfolio();
        Task<IReadOnlyList<Repository>> SearchRepositories(string? repoName, string? language, string? user);
    }
}
