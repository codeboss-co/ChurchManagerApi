using ChurchManager.Domain.Features.Communication.Services;
using ChurchManager.Infrastructure.Abstractions.Communication;
using ChurchManager.Infrastructure.Shared.Email;
using ChurchManager.Infrastructure.Shared.Templating;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Infrastructure.Shared._DependencyInjection
{
    public class CommunicationsDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            // Communications
            services.AddSingleton<ITemplateParser, DotLiquidTemplateParser>();
            services.AddSingleton<IEmailSender>(sp =>
            {
                // AWS Configuration
                var accessKey = configuration["AWS:AccessKey"];
                var secretKey = configuration["AWS:SecretKey"];

                return new AwsSesEmailSender(accessKey, secretKey);
            } );
        }
    }
}
