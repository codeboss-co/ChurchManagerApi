using ChurchManager.Infrastructure.Shared.WebPush;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Infrastructure.Shared._DependencyInjection
{
    public class WebPushDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.Configure<PushNotificationsOptions>(configuration.GetSection(nameof(PushNotificationsOptions)));

            services.AddPushServiceClient(options =>
            {
                IConfigurationSection pushOptions = configuration.GetSection(nameof(PushNotificationsOptions));

                options.Subject = pushOptions.GetValue<string>(nameof(options.Subject));
                options.PublicKey = pushOptions.GetValue<string>(nameof(options.PublicKey));
                options.PrivateKey = pushOptions.GetValue<string>(nameof(options.PrivateKey));
            });

            //services.AddHttpClient<PushServiceClient>();
        }
    }
}
