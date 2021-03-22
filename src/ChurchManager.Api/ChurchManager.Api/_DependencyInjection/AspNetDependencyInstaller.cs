using ChurchManager.Api.Extensions;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Api._DependencyInjection
{
    public class AspNetDependencyInstaller : IDependencyInstaller
    {
        private const string Prefix = "api";

        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddCorsExtension();
            services.AddControllersExtension(Prefix);
            services.AddSwaggerExtension();

            // API version
            services.AddApiVersioningExtension();
            // API explorer
            services.AddMvcCore()
                .AddApiExplorer();
            // API explorer version
            services.AddVersionedApiExplorerExtension();

            services.AddHealthChecks();
        }
    }
}
