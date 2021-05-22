using ChurchManager.Domain;
using ChurchManager.Domain.Features.Communication.Services;
using ChurchManager.Infrastructure.Shared.WebPush;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WebPush;

namespace ChurchManager.Infrastructure.Shared._DependencyInjection
{
    public class WebPushDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.Configure<PushNotificationsOptions>(configuration.GetSection(nameof(PushNotificationsOptions)));

            services.AddSingleton<IPushServiceClient>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<PushNotificationsOptions>>();
                var vapidDetails = new VapidDetails(options.Value.Subject, options.Value.PublicKey,
                    options.Value.PrivateKey);

                var webPushClient = new WebPushClient();
                webPushClient.SetVapidDetails(vapidDetails);

                return new PushServiceClient(webPushClient);
            });

            /*services.AddSingleton<WebPushClient>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<PushNotificationsOptions>>();
                var vapidDetails = new VapidDetails(options.Value.Subject, options.Value.PublicKey,
                    options.Value.PrivateKey);

                var client = new WebPushClient();
                client.SetVapidDetails(vapidDetails);

                return client;
            });*/

            /*services.AddPushServiceClient(options =>
            {
                IConfigurationSection pushOptions = configuration.GetSection(nameof(PushNotificationsOptions));

                options.Subject = pushOptions.GetValue<string>(nameof(options.Subject));
                options.PublicKey = pushOptions.GetValue<string>(nameof(options.PublicKey));
                options.PrivateKey = pushOptions.GetValue<string>(nameof(options.PrivateKey));
            });*/

            //services.AddHttpClient<PushServiceClient>();
        }
    }
}
