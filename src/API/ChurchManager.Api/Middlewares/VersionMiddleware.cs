using System.Threading.Tasks;
using CodeBoss.AspNetCore;
using Microsoft.AspNetCore.Http;

namespace ChurchManager.Api.Middlewares
{
    /// <summary>
    /// Adds X-Version Http Headers to response
    /// </summary>
    public class VersionMiddleware
    {
        private readonly RequestDelegate _next;

        public VersionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IBuildVersionInfo buildInfo)
        {
            context.Response.Headers.Add("X-Version", $"{buildInfo.Version}");
            await _next(context);
        }
    }
}
