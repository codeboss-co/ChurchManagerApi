using ChurchManager.Domain.Features.SharedKernel.MultiTenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class TenantsController : BaseApiController
    {
        private readonly ITenantProvider _provider;

        public TenantsController(ITenantProvider provider) => _provider = provider;

        [HttpGet]
        public IActionResult GetTenants() => Ok(_provider.Tenants());
    }
}
