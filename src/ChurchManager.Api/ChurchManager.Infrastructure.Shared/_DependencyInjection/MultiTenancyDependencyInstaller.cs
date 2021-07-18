using CodeBoss.AspNetCore.DependencyInjection;
using CodeBoss.MultiTenant;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Infrastructure.Shared._DependencyInjection
{
    public class MultiTenancyDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddCodeBossMultiTenancy(configuration);

            // Injected into db context to provide UserLoginId info
            services.AddScoped<ITenantCurrentUser, SimpleCurrentUser>();

        }
    }
}
