using System.Linq;
using Codeboss.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UtilityController : ControllerBase
    {
        private readonly ILogger<UtilityController> _logger;
        private readonly ICurrentUser _currentUser;

        public UtilityController(ILogger<UtilityController> logger, ICurrentUser currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("auth")]
        [Authorize]
        public IActionResult AuthTest()
        {
            return Ok(User.Claims.Select( x => new { Name=x.Type, x.Value}));
        }
    }
}
