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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddConvey().Build())
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddEnvironmentVariables();
                } )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    var environmentName = context.HostingEnvironment.EnvironmentName;
                    Console.WriteLine($"** Environment: [{environmentName}], " +
                                      $"ASPNETCORE_ENVIRONMENT: [{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}] **" +
                                      $"AWS_ACCESS_KEY : [{Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")}] **"
                    );
                    // Pull settings from AWS parameter store
                    ConfigureAwsParameterStore(config, environmentName);

                    // https://github.com/npgsql/efcore.pg/issues/2000
                    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                })
                .UseLogging();

            void ConfigureAwsParameterStore(IConfigurationBuilder configurationBuilder, string environment)
            {
                configurationBuilder
                    .AddEnvironmentVariables()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", true, true);

                var configuration = configurationBuilder.Build();

                // AWS Configuration
                var awsOptions = configuration.GetAWSOptions();
                //var accessKey = configuration["AWS:AccessKey"];
                //var secretKey = configuration["AWS:SecretKey"];
                //awsOptions.Credentials = new BasicAWSCredentials(accessKey, secretKey);
                awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();

                // AWS Parameter Store
                configurationBuilder.AddSystemsManager(
                    path: $"/{AppName}/{environment}",
                    awsOptions: awsOptions,
                    reloadAfter: TimeSpan.FromMinutes(5));
            }

        }
    }
}