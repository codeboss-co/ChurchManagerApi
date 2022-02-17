using CodeBoss.AspNetCore.DependencyInjection;
using Convey;
using Flagsmith;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Infrastructure.Shared.FeatureFlags
{
    public class FeatureFlagsDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            // Get from configuration
            var options = configuration.GetOptions<FeatureFlagOptions>(nameof(FeatureFlagOptions));
            // Add to DI
            services.Configure<FeatureFlagOptions>(configuration.GetSection(nameof(FeatureFlagOptions)));

            if (options.Enabled)
            {
                var config = new FlagsmithConfiguration()
                {
                    ApiUrl = options.ApiUrl ?? "https://api.flagsmith.com/api/v1/",
                    EnvironmentKey = options.Key
                };

                var client = new FlagsmithClient(config);
            }
        }
    }
}
