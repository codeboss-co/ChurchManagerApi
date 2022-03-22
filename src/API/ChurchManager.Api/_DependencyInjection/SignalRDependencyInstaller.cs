using ChurchManager.Api.Authorization;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.AspNetCore.SignalR;

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
