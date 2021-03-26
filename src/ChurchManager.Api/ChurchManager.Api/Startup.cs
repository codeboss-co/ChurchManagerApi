using ChurchManager.Api.Extensions;
using ChurchManager.Api.Hubs;
using ChurchManager.Application;
using ChurchManager.Infrastructure.Persistence;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ChurchManager.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }


        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPersistenceInfrastructure(Configuration, Environment);
            services.AddApplicationLayer();

            services.InstallServicesInAssemblies(Configuration, Environment, typeof(Startup).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(ApiRoutes.DefaultCorsPolicy);
            app.UseSwaggerExtension();
            app.UseSerilogRequestLogging();

            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseErrorHandlingMiddleware();

            app.UseHealthChecks(ApiRoutes.HealthChecks.DefaultUrl);

            app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllers();
                 endpoints.MapHub<NotificationHub>(ApiRoutes.Hubs.NotificationHub);
             });
        }
    }
}
