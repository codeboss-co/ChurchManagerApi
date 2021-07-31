using ChurchManager.Api.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ChurchManager.Api.Extensions
{
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/swagger/{documentname}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "ChurchManager.Api v1");
                c.RoutePrefix = "api/swagger";
            });
        }
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<VersionMiddleware>();
        }

        public static void UseMultiTenant(this IApplicationBuilder app)
        {
            app.UseMiddleware<TenantIdentifierMiddleware>();
        }
    }
}
