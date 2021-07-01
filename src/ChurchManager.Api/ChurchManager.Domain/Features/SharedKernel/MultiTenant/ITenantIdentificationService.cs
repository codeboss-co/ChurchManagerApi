namespace ChurchManager.Domain.Features.SharedKernel.MultiTenant
{
    public interface ITenantIdentificationService
    {
        Tenant CurrentTenant();
    }
}
