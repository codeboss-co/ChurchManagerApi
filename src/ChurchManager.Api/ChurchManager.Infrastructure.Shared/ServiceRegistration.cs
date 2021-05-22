using ChurchManager.Infrastructure.Abstractions.Security;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.InstallServicesInAssemblies(configuration, environment, typeof(ServiceRegistration).Assembly);

            services.AddSingleton<ITokenService, TokenService>();
        }
    }
}
