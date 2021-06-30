namespace ChurchManager.Domain.Features.SharedKernel.MultiTenant
{
    public interface ITenantProvider
    {
        public bool Enabled { get;}

        Tenant[] Tenants();
    }
}
