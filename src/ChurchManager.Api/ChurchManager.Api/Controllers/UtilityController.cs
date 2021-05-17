using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Common;
using ChurchManager.Application.Features.Groups.Queries.GroupsForPerson;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class UtilityController : ControllerBase
    {
        private readonly ILogger<UtilityController> _logger;
        private readonly IMediator _mediator;
        private readonly ICognitoCurrentUser _currentUser;
        private readonly IChurchManagerDbContext _dbContext;

        public UtilityController(
            ILogger<UtilityController> logger,
            IMediator mediator,
            ICognitoCurrentUser currentUser,
            IChurchManagerDbContext dbContext)
        {
            _logger = logger;
            _mediator = mediator;
            _currentUser = currentUser;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok("All good in the hood!");
        }

        /// <summary>
        /// Checks database migrations status and performs basic db query
        /// </summary>
        /// <returns>Returns the database status</returns>
        /// <response code="200">Returns the database status</response>
        [HttpGet("db")]
        public async Task<IActionResult> DatabaseCheck(CancellationToken token)
        {
            var applied = await _dbContext.Database.GetAppliedMigrationsAsync(token);
            var pending = await _dbContext.Database.GetPendingMigrationsAsync(token);
            var query = await _mediator.Send(new GroupsForPersonQuery(1), token);

            var result = new
            {
                database = new { applied , pending },
                query
            };

            return Ok(result);
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
