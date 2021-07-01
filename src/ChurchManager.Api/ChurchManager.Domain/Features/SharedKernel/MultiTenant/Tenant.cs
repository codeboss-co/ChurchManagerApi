using System.ComponentModel.DataAnnotations;

namespace ChurchManager.Domain.Features.SharedKernel.MultiTenant
{
    public class Tenant
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string ConnectionString { get; set; }
    }
}
