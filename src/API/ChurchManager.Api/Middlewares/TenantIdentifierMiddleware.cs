using System.Linq;
using System.Threading.Tasks;
using CodeBoss.MultiTenant;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Api.Middlewares
{
    public class TenantIdentifierMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITenantProvider _tenantProvider;
        private readonly ILogger<TenantIdentifierMiddleware> _logger;

        public TenantIdentifierMiddleware(
            RequestDelegate next,
            ITenantProvider tenantProvider,
            ILogger<TenantIdentifierMiddleware> logger
            )
        {
            _next = next;
            _tenantProvider = tenantProvider;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var tenantName = context.Request.Query["tenant"].ToString();

            if(string.IsNullOrWhiteSpace(tenantName))
            {
                if(context.User.Identity is not null && context.User.Identity.IsAuthenticated)
                {
                    tenantName = context.User.Claims.FirstOrDefault(c => c.Type == "Tenant")?.Value;
                }
            }

            var tenant = _tenantProvider.Get(tenantName);
            _tenantProvider.CurrentTenant = tenant;

            if(tenant is not null)
            {
                _logger.LogInformation($"Tenant found in query string: {tenant.Name}");
            }

            await _next(context);
        }
    }
}
