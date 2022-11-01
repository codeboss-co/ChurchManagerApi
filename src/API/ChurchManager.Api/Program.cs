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
                   
                    // Validate Environment Variables Needed
                    ValidateEnvironmentVariables(environmentName);

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

            void ValidateEnvironmentVariables(string environment)
            {
                Console.WriteLine($"** Environment: [{environment}], " +
                                  $"ASPNETCORE_ENVIRONMENT: [{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}]  " +
                                  $"AWS_ACCESS_KEY : [{Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")}]  " +
                                  $"AWS_REGION  : [{Environment.GetEnvironmentVariable("AWS_REGION")}]  "
                );

                _ = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? throw new ArgumentNullException("ASPNETCORE_ENVIRONMENT");
                _ = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY") ?? throw new ArgumentNullException("AWS_ACCESS_KEY");
                _ = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") ?? throw new ArgumentNullException("AWS_ACCESS_KEY_ID");
                _ = Environment.GetEnvironmentVariable("AWS_REGION ") ?? throw new ArgumentNullException("AWS_REGION ");

            }
        }
    }
}