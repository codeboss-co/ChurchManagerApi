using ChurchManager.Domain.Features.SharedKernel.MultiTenant;
using Microsoft.Extensions.Options;

namespace ChurchManager.Infrastructure.Shared.MultiTenant
{
    public class FileTenantProvider : ITenantProvider
    {

        private readonly MultiTenantOptions _options;

        public FileTenantProvider(IOptions<MultiTenantOptions> options)
        {
            _options = options.Value;
        }

        public Tenant[] Tenants() => _options.Tenants;
    }
}
