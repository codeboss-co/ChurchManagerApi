using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Handlers;
using ChurchManager.Core;
using CodeBoss.CQRS.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class UtilityController : ControllerBase
    {
        private readonly ILogger<UtilityController> _logger;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICognitoCurrentUser _currentUser;

        public UtilityController(
            ILogger<UtilityController> logger,
            IQueryDispatcher queryDispatcher,
            ICognitoCurrentUser currentUser)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
            _currentUser = currentUser;
        }

        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok("All good in the hood!");
        }

        [HttpGet("db")]
        public async Task<IActionResult> DatabaseCheck()
        {
            return Ok(await _queryDispatcher.QueryAsync(new GroupsForPersonQuery(1), CancellationToken.None));
        }

        [HttpGet("auth")]
        [Authorize]
        public async Task<IActionResult> AuthTest()
        {
            var person = await _currentUser.CurrentPerson.Value;
            return Ok(User.Claims.Select( x => new { Name=x.Type, x.Value}));
        }
    }
}
