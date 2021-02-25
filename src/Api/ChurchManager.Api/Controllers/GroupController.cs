using System.Threading;
using System.Threading.Tasks;
using Application.Handlers;
using CodeBoss.CQRS.Commands;
using CodeBoss.CQRS.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Security;

namespace ChurchManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICognitoCurrentUser _currentUser;

        public GroupController(
            ILogger<GroupController> logger,
            IQueryDispatcher queryDispatcher,
            ICognitoCurrentUser currentUser)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
            _currentUser = currentUser;
        }

        [HttpGet("{personId}")]
        public async Task<IActionResult> GetAllPersonsGroups(int personId, CancellationToken token)
        {
            var groups = await _queryDispatcher.QueryAsync(new GroupsForPersonQuery(personId), token);
            return Ok(groups);
        }

        /// <summary>
        /// Current User Groups
        /// </summary>
        [HttpGet("current-person")]
        [Authorize]
        public async Task<IActionResult> GetAllCurrentUsersGroups(CancellationToken token)
        {
            var currentPerson = _currentUser.CurrentPerson.Value.Result;
            var groups = await _queryDispatcher.QueryAsync(new GroupsForPersonQuery(currentPerson.PersonId), token);
            return Ok(groups);
        }
    }
}
