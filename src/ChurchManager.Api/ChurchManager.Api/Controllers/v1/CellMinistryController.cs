using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Groups.Queries.BrowseGroupAttendance;
using ChurchManager.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class CellMinistryController : BaseApiController
    {
        private readonly ICognitoCurrentUser _currentUser;

        public CellMinistryController(ICognitoCurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpPost("browse")]
        public async Task<IActionResult> BrowseCellGroups([FromBody] BrowseGroupAttendanceQuery query, CancellationToken token)
        {
            var groups = await Mediator.Send(query, token);
            return Ok(groups);
        }
    }
}
