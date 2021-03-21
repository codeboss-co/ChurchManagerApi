using System;
using ChurchManager.Api.Hubs;
using CodeBoss.AspNetCore.DependencyInjection;
using MassTransit;
using MassTransit.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Api._DependencyInjection
{
    public class MassTransitDependencyInstaller : IDependencyInstaller
    {
        public static string RabbitMqUri = Environment.GetEnvironmentVariable("RABBITMQ_URI") ?? "amqp://guest:guest@localhost:5672";

        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            #region MassTransit

            services.AddSignalR();
            services.AddMassTransit(x =>
            {
                // ** Add Hubs Here **
                x.AddSignalRHub<NotificationHub>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(RabbitMqUri), h => { });

                    cfg.UseHealthCheck(provider);

                    cfg.ConfigureEndpoints(provider);

                    cfg.UseJsonSerializer();
                }));

            });

            services.AddMassTransitHostedService();

            #endregion
        }
    }
}
