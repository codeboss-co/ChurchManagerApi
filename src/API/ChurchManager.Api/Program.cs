using Amazon.Runtime;
using Convey;
using Convey.Logging;

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
                .ConfigureAppConfiguration((context, config) =>
                {
                    var environmentName = context.HostingEnvironment.EnvironmentName;
                    Console.WriteLine($"** Environment: [{environmentName}], ASPNETCORE_ENVIRONMENT: [{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}] **");
                    // We pull settings from AWS parameter store if not running locally
                    if(!context.HostingEnvironment.IsDevelopment())
                    {
                        config
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{environmentName}.json", true, true);

                        var configuration = config.Build();

                        // AWS Configuration
                        var awsOptions = configuration.GetAWSOptions();
                        var accessKey = configuration["AWS:AccessKey"];
                        var secretKey = configuration["AWS:SecretKey"];
                        awsOptions.Credentials = new BasicAWSCredentials(accessKey, secretKey);
                        // AWS Parameter Store
                        config.AddSystemsManager(
                            path: $"/{AppName}/{environmentName}", 
                            awsOptions: awsOptions,
                            reloadAfter: TimeSpan.FromMinutes(5));
                    }

                    // https://github.com/npgsql/efcore.pg/issues/2000
                    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                })
                .UseLogging();
    }
}