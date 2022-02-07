using ChurchManager.Domain.Features.Communication.Services;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Infrastructure.Shared.SignalR
{
    public class MassTransitSignalRDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            // MassTransit SignalR Hubs
            //services.AddTransient<IPushNotificationsService<INotification, IPublishEndpoint>, MassTransitSignalRPushNotificationsService>();
            services.AddTransient<IPushNotificationsService, MassTransitSignalRPushNotificationsService>(); //<-- use this for easy referencing

            services.AddTransient<IUserNotificationsHubService, MassTransitUserNotificationsSignalRHubService>();
        }
    }
}
