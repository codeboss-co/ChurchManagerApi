using ChurchManager.Api.Middlewares;
using CodeBoss.AspNetCore;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Api._DependencyInjection
{
    public class BuildVersionDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddSingleton<IBuildVersionInfo>(sp =>
            {
                var host = sp.GetRequiredService<IWebHostEnvironment>();
                return new BuildVersionInfo(host.ContentRootPath);
            });
        }
    }
}
