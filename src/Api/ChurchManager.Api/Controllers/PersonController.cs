using System.Threading;
using System.Threading.Tasks;
using CodeBoss.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using People.Application.Handlers;

namespace ChurchManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IQueryDispatcher _queryDispatcher;

        public PersonController(
            ILogger<PersonController> logger,
            IQueryDispatcher queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("userlogin/{userLoginId}")]
        public async Task<IActionResult> GetUserProfileByUserLogin(string userLoginId, CancellationToken token)
        {
            var person = await _queryDispatcher.QueryAsync(new ProfileByUsernameQuery(userLoginId), token);
            return Ok(person);
        }
    }
}
