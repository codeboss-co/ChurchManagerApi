namespace ChurchManager.Domain.Features.SharedKernel.MultiTenant
{
    public interface ITenantProvider
    {
        public bool Enabled { get; }
        Tenant[] Tenants();
        Tenant Get(string name);
        Tenant CurrentTenant { get; set; }
    }
}
