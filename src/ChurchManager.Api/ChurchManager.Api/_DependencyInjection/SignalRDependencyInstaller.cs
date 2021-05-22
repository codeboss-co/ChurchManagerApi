using ChurchManager.Api.Authorization;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Api._DependencyInjection
{
    public class SignalRDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddSignalR();

            // Change to use name as the user identifier for SignalR
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
        }
    }
}
