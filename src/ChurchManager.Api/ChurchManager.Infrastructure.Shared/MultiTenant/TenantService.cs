using System.Linq;
using ChurchManager.Domain.Features.SharedKernel.MultiTenant;
using Microsoft.AspNetCore.Http;

namespace ChurchManager.Infrastructure.Shared.MultiTenant
{
    public class QueryStringTenantIdentificationService : ITenantIdentificationService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ITenantProvider _provider;

        public QueryStringTenantIdentificationService(IHttpContextAccessor accessor, ITenantProvider provider)
        {
            _accessor = accessor;
            _provider = provider;
        }

        public Tenant CurrentTenant()
        {
            if (_accessor.HttpContext != null)
            {
                var tenantName = _accessor.HttpContext.Request.Query["Tenant"].ToString();

                if(!string.IsNullOrWhiteSpace(tenantName))
                {
                    return _provider.Tenant(tenantName);
                }
            }

            return _provider.Tenants().FirstOrDefault();
        }
    }
}
