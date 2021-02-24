using System.Threading;
using System.Threading.Tasks;
using Application.Handlers;
using CodeBoss.CQRS.Commands;
using CodeBoss.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IQueryDispatcher _queryDispatcher;

        public GroupController(
            ILogger<GroupController> logger,
            IQueryDispatcher queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("{personId}")]
        public async Task<IActionResult> GetAllPersonsGroups(int personId, CancellationToken token)
        {
            var groups = await _queryDispatcher.QueryAsync(new GroupsForPersonQuery(personId), token);
            return Ok(groups);
        }
    }
}
