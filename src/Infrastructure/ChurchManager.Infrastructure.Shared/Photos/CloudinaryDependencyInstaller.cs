using ChurchManager.Domain.Features.People.Services;
using CloudinaryDotNet;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ChurchManager.Infrastructure.Shared.Photos
{
    public class CloudinaryDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.Configure<CloudinarySettings>(configuration.GetSection(nameof(CloudinarySettings)));

            services.AddScoped<Cloudinary>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
                var account = new Account(options.CloudName, options.ApiKey, options.ApiSecret);
                return new Cloudinary(account);
            });

            services.AddScoped<IPhotoService, CloudinaryPhotoService>();
        }
    }
}
