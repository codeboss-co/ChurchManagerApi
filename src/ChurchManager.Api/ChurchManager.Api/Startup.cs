using ChurchManager.Api.Extensions;
using ChurchManager.Application;
using ChurchManager.Infrastructure.Persistence;
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
        private const string Prefix = "api";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsExtension();
            services.AddControllersExtension(Prefix);
            services.AddSwaggerExtension();

            services.AddPersistenceInfrastructure(Configuration);
            services.AddApplicationLayer();

            services.AddHealthChecks();
            // API version
            services.AddApiVersioningExtension();
            // API explorer
            services.AddMvcCore()
                .AddApiExplorer();
            // API explorer version
            services.AddVersionedApiExplorerExtension();

            services.AddAuth(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");
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
            app.UseHealthChecks("/api/health");

            app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllers();
             });
        }
    }
}
