using CV_Site.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace cv_site.Service
{
   public static class Extensions
    {
        public static void AddGitHubIntegration(this IServiceCollection services,Action<GitHubIntegrationOption>configureOptions )
        {
            services.Configure(configureOptions);
            services.AddScoped<IGitHubService, GitHubService>();
        }
    }
}
