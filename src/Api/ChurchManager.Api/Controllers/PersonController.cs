using System.Threading;
using System.Threading.Tasks;
using Application.Handlers;
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
        public async Task<IActionResult> GetPersonByUsername(string userLoginId, CancellationToken token)
        {
            var groups = await _queryDispatcher.QueryAsync(new PersonByUsernameQuery(userLoginId), token);
            return Ok(groups);
        }
    }
}
