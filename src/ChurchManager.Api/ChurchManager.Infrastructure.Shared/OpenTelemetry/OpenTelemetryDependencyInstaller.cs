using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ChurchManager.Infrastructure.Shared.OpenTelemetry
{
    public class OpenTelemetryDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            // Configure
            services.Configure<AspNetCoreInstrumentationOptions>(options =>
            {
                options.Filter = (httpContext) =>
                {
                    // only collect telemetry about the following requests
                    var path = httpContext.Request.Path;
                    return path.HasValue && (
                        !path.Value.Contains("aspnetcore-browser-refresh") &&
                        !path.Value.Contains("swagger") 
                    );
                };
            });

            services.AddOpenTelemetryTracing(builder => builder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("ChurchManager"))
                .AddSource("ChurchManager.Application") // when we manually create activities, we need to setup the sources here
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation(options => options.SetDbStatementForText = true)
                .AddJaegerExporter(options =>
                {
                    // not needed, it's the default
                    //options.AgentHost = "localhost";
                    //options.AgentPort = 6831;
                })
            );
        }
    }
}
