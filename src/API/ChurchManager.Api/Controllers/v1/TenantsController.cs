using System.Linq;
using CodeBoss.MultiTenant;
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
        [AllowAnonymous]
        public IActionResult GetTenants()
        {
            var tenants = _provider.Tenants();
            return Ok(tenants.Select(x => new SafeTenant(x.Name)));
        }

        /// <summary>
        /// Publicly safe tenant without connection string
        /// </summary>
        public record SafeTenant(string Name);
    }
}
