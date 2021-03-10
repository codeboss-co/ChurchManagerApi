using System;
using Convey;
using Convey.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Api
{
    public class Program
    {
        public const string AppName = "ChurchManager";

        public static void Main(string[] args)
        {
            Console.Title = AppName;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddConvey().Build())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    // We pull settings from AWS parameter store if not running locally
                    if(!context.HostingEnvironment.IsDevelopment())
                    {
                        var configuration = context.Configuration;
                        var environmentName = context.HostingEnvironment.EnvironmentName;
                        // AWS Configuration
                        var awsOptions = configuration.GetAWSOptions();
                        builder.AddSystemsManager($"/{AppName}/{environmentName}", awsOptions);
                    }
                })
                .UseLogging();
    }
}