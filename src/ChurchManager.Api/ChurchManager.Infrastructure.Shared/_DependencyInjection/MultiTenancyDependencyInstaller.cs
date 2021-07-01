using ChurchManager.Domain.Features.SharedKernel.MultiTenant;
using ChurchManager.Infrastructure.Shared.MultiTenant;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Infrastructure.Shared._DependencyInjection
{
    public class MultiTenancyDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.Configure<MultiTenantOptions>(configuration.GetSection(nameof(MultiTenantOptions)));

            services.AddSingleton<ITenantProvider, FileTenantProvider>();
        }
    }
}
