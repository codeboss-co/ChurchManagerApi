using Bugsnag.AspNet.Core;
using ChurchManager.Api.Models;
using CodeBoss.AspNetCore.DependencyInjection;
using Convey;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Api._DependencyInjection
{
    public class BugsnagDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            var options = configuration.GetOptions<BugsnagOptions>(nameof(BugsnagOptions));

            services.Configure<BugsnagOptions>(configuration.GetSection(nameof(BugsnagOptions)));

            if (options.Enabled)
            {
                services.AddBugsnag(config => {
                    config.ApiKey = options.ApiKey;
                });
            }
        }
    }
}
