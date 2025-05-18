using cv_site.Service;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV_Site.Service
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubIntegrationOption _gitHubInetgrationOptions;
        private readonly IMemoryCache _memoryCache;

        private const string CacheKey = "GitHub_Portfolio_";

        public GitHubService(IOptions<GitHubIntegrationOption> options, IMemoryCache memoryCache)
        {
            _gitHubInetgrationOptions = options.Value;
            _client = new GitHubClient(new ProductHeaderValue("YourAppName"))
            {
                Credentials = new Credentials(_gitHubInetgrationOptions.GitHubToken)
            };
            _memoryCache = memoryCache;
        }

        public async Task<List<GitHubRepositoryModel>> GetPortfolio()
        {
            string cacheKey = $"{CacheKey}{_gitHubInetgrationOptions.UserName}";

            if (_memoryCache.TryGetValue(cacheKey, out List<GitHubRepositoryModel> cachedRepos))
            {
                return cachedRepos;
            }

            var repositories = await _client.Repository.GetAllForUser(_gitHubInetgrationOptions.UserName);

            var detailedRepos = repositories.Select(repo => new GitHubRepositoryModel
            {
                Name = repo.Name,
                HtmlUrl = repo.HtmlUrl,
                Language = repo.Language,
                StargazersCount = repo.StargazersCount,
                ForksCount = repo.ForksCount,
                OpenIssuesCount = repo.OpenIssuesCount,
                PushedAt = repo.PushedAt,
                CreatedAt = repo.CreatedAt,
                UpdatedAt = repo.UpdatedAt,
                Owner = repo.Owner.Login,
                Archived = repo.Archived,
                Private = repo.Private
            }).ToList();

            _memoryCache.Set(cacheKey, detailedRepos, TimeSpan.FromMinutes(5));

            return detailedRepos;
        }

        public async Task<IReadOnlyList<Repository>> SearchRepositories(string? repoName, string? language, string? user)
        {
            if (string.IsNullOrWhiteSpace(repoName) && string.IsNullOrWhiteSpace(language) && string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentException("At least one search parameter (repoName, language, or user) must be provided.");
            }

            var query = "";

            if (!string.IsNullOrWhiteSpace(repoName))
            {
                query += $"{repoName} ";
            }

            if (!string.IsNullOrWhiteSpace(language))
            {
                query += $"language:{language} ";
            }

            if (!string.IsNullOrWhiteSpace(user))
            {
                query += $"user:{user}";
            }

            var request = new SearchRepositoriesRequest(query.Trim());

            var result = await _client.Search.SearchRepo(request);

            return result.Items;
        }

    }
}
