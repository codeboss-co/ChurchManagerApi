using ChurchManager.Domain.Features.SharedKernel.MultiTenant;

namespace ChurchManager.Infrastructure.Shared.MultiTenant
{
    public class MultiTenantOptions
    {
        public bool Enabled { get; set; }
        public Tenant[] Tenants { get; set; }
    }
}
