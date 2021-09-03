using System;
using Bugsnag;
using Bugsnag.AspNet.Core;
using ChurchManager.Domain.Common.Configuration;
using CodeBoss.AspNetCore;
using CodeBoss.AspNetCore.DependencyInjection;
using Convey;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ChurchManager.Api._DependencyInjection
{
    public class BugsnagDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            // Get from configuration
            var options = configuration.GetOptions<BugsnagOptions>(nameof(BugsnagOptions));
            // Add to DI
            services.Configure<BugsnagOptions>(configuration.GetSection(nameof(BugsnagOptions)));

            if (options.Enabled)
            {
                services.AddSingleton<IStartupFilter, BugsnagStartupFilter>();
                services.AddScoped<IClient, Client>((context =>
                {
                    //  Version should be '20211203-1-Production'
                    var buildInfo = context.GetRequiredService<IBuildVersionInfo>();

                    // Set Church Manager variables
                    options.AppVersion = buildInfo.Version;
                    options.IgnoreClasses = new[] { typeof(OperationCanceledException) };

                    return new Client(options);
                }));
            }
        }
    }
}
