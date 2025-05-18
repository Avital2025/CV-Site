using cv_site.Service;
using CV_Site.Service;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CV_Site.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public GitHubController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        /// <summary>
        /// שליפת כל הריפוזיטוריז של המשתמש שהוגדר בקונפיגורציה
        /// </summary>
        [HttpGet("portfolio")]
        public async Task<ActionResult<List<GitHubRepositoryModel>>> GetPortfolio()
        {
            var repositories = await _gitHubService.GetPortfolio();
            if (repositories == null || repositories.Count == 0)
            {
                return NotFound("לא נמצאו ריפוזיטוריז למשתמש זה.");
            }
            return Ok(repositories);
        }

        /// <summary>
        /// חיפוש ריפוזיטוריז לפי שם, שפה ומשתמש
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<List<GitHubRepositoryModel>>> SearchRepositories(
            [FromQuery] string? repoName,
            [FromQuery] string? language,
            [FromQuery] string? user)
        {
            var repositories = await _gitHubService.SearchRepositories(repoName, language, user);

            if (repositories == null || repositories.Count == 0)
            {
                return NotFound("לא נמצאו תוצאות לחיפוש המבוקש.");
            }

            // מיפוי התוצאות למודל שמותאם להצגה
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

            return Ok(detailedRepos);
        }
    }
}
