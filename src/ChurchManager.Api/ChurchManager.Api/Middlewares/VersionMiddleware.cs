using System;
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
        private readonly string _version = $"{DateTime.UtcNow:yyyyMMdd}-local";

        public VersionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IBuildVersionInfo buildInfo)
        {
            context.Response.Headers.Add("X-Version", $"{_version}");
            await _next(context);
        }
    }
}
