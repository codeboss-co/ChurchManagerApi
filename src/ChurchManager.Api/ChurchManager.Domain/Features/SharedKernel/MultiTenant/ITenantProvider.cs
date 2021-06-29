namespace ChurchManager.Domain.Features.SharedKernel.MultiTenant
{
    public interface ITenantProvider
    {
        Tenant[] Tenants();
    }
}
